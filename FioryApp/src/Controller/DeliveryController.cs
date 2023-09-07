using FioryApp.Entity;
using FioryApp.Service;

namespace FioryApp.Controller;

public class DeliveryController
{
    public OrderEntity orderObj { get; set; }
    public OrderEntity dispatchObj { get; private set; }

    public int totalProductsToScan { get; private set; }
    public int totalProductsScanned { get; private set; }
    public decimal efficiency { get; private set; }
    public List<ProductEntity> productsOrder { get; private set; }
    private List<OrderProduct> productsDispatch { get; set; } //Pending to Set


    private readonly SessionService _sessionService;
    private readonly List<ProductEntity> _productList;

    public DeliveryController(SessionService sessionService)
    {
        _sessionService = sessionService;
        _productList = _sessionService.productsList;
    }

    public void SetDeliveryController(OrderEntity contapymeOrder, int order)
    {

        LoggerService.CreateLogFile().GetAwaiter().GetResult();
        LoggerService.Info("Scan Operation: the operation for scanning is started for order " + order);

        orderObj = contapymeOrder;
        SetProductsOrder(orderObj.listaproductos);

        dispatchObj = new OrderEntity
        {
            encabezado = orderObj.encabezado,
            liquidacion = new OrderInvoiceSettlement(),
            datosprincipales = orderObj.datosprincipales,
            listaproductos = null,
            qoprsok = orderObj.qoprsok
        };

        productsDispatch = new List<OrderProduct>();
    }

    public void SetDispatch()
    {
        dispatchObj.listaproductos = productsDispatch;
        dispatchObj.encabezado.iusuarioult = "WEBAPI";
    }

    private void SetProductsOrder(List<OrderProduct> products)
    {
        // Step 1: Group and Sum Duplicates in the First Object
        var groupedProducts = products
            .GroupBy(p => p.irecurso)
            .Select(group => new OrderProduct
            {
                irecurso = group.Key,
                qrecurso = group.Sum(p => p.qrecurso)
            })
            .ToList();

        // Step 2: Create the Second Object from the First
        var productEntities = groupedProducts.Select(orderProduct => new ProductEntity
        {
            name = orderProduct.irecurso, // Assign irecurso to name
            barcode = "", // You can assign an empty string or specify a value for barcode
            code = orderProduct.irecurso, // Assign irecurso to code
            quantity = 0, // You can assign 0 or specify a value for quantity
            requested = orderProduct.qrecurso // Assign summed qrecurso to requested
        }).ToList();

        // Step 3: Assign the Second Object to the Property
        productsOrder = productEntities;
        totalProductsToScan = productsOrder.Sum(product => product.requested);
    }

    public string SetProductsDispatched(string targetBarcode)
    {
        string message = "";
        try
        {
            // Step 1: Find the product in the Master Data
            ProductEntity foundProduct = _productList.FirstOrDefault(product => product.barcode == targetBarcode);

            if (foundProduct != null)
            {
                // Step 2: Find the product in the Order List
                OrderProduct foundProductInOrder = orderObj.listaproductos.FirstOrDefault(product => product.irecurso == foundProduct.code);

                if (foundProductInOrder != null)
                {
                    if (productsDispatch.Count == 0)
                    {
                        productsDispatch.Add(new OrderProduct
                        {
                            irecurso = foundProductInOrder.irecurso,
                            itiporec = foundProductInOrder.itiporec,
                            icc = foundProductInOrder.icc,
                            sobserv = foundProductInOrder.sobserv,
                            dato1 = foundProductInOrder.dato1,
                            dato2 = foundProductInOrder.dato2,
                            dato3 = foundProductInOrder.dato3,
                            dato4 = foundProductInOrder.dato4,
                            dato5 = foundProductInOrder.dato5,
                            dato6 = foundProductInOrder.dato6,
                            iinventario = foundProductInOrder.iinventario,
                            qrecurso = foundProductInOrder.qrecurso,
                            mprecio = foundProductInOrder.mprecio,
                            qporcdescuento = foundProductInOrder.qporcdescuento,
                            qporciva = foundProductInOrder.qporciva,
                            mvrtotal = foundProductInOrder.mvrtotal,
                            valor1 = foundProductInOrder.valor1,
                            valor2 = foundProductInOrder.valor2,
                            valor3 = foundProductInOrder.valor3,
                            valor4 = foundProductInOrder.valor4,
                            qrecurso2 = foundProductInOrder.qrecurso2,
                        });
                    }
                    else
                    {
                        int foundProductIndex = productsDispatch.FindIndex(product => product.irecurso == foundProduct.code);

                        if (foundProductIndex >= 0)
                        {
                            productsDispatch[foundProductIndex].qrecurso = productsDispatch[foundProductIndex].qrecurso + 1;
                            decimal newPrice = productsDispatch[foundProductIndex].qrecurso *
                                productsDispatch[foundProductIndex]
                                .mprecio;

                            decimal discount = productsDispatch[foundProductIndex].qporcdescuento / 100;
                            productsDispatch[foundProductIndex].mvrtotal = newPrice - (newPrice * discount);

                        }
                        else
                        {
                            productsDispatch.Add(new OrderProduct
                            {
                                irecurso = foundProductInOrder.irecurso,
                                itiporec = foundProductInOrder.itiporec,
                                icc = foundProductInOrder.icc,
                                sobserv = foundProductInOrder.sobserv,
                                dato1 = foundProductInOrder.dato1,
                                dato2 = foundProductInOrder.dato2,
                                dato3 = foundProductInOrder.dato3,
                                dato4 = foundProductInOrder.dato4,
                                dato5 = foundProductInOrder.dato5,
                                dato6 = foundProductInOrder.dato6,
                                iinventario = foundProductInOrder.iinventario,
                                qrecurso = foundProductInOrder.qrecurso,
                                mprecio = foundProductInOrder.mprecio,
                                qporcdescuento = foundProductInOrder.qporcdescuento,
                                qporciva = foundProductInOrder.qporciva,
                                mvrtotal = foundProductInOrder.mvrtotal,
                                valor1 = foundProductInOrder.valor1,
                                valor2 = foundProductInOrder.valor2,
                                valor3 = foundProductInOrder.valor3,
                                valor4 = foundProductInOrder.valor4,
                                qrecurso2 = foundProductInOrder.qrecurso2,
                            });
                        }
                    }

                    productsOrder[productsOrder.FindIndex(product => product.code == foundProduct.code)].quantity++;
                    totalProductsScanned++;
                    efficiency = Math.Round((decimal)totalProductsScanned / totalProductsToScan, 3);

                    message = "Producto agregado";
                }
                else
                {
                    message = "Producto no solicitado";
                }
            }
            else
            {
                message = "Producto no encontrado";
            }
        
        }
        catch (Exception e)
        {
            LoggerService.Warning("Scanning: " + e.Message);
            message = "Error de ejecución";
        }

        return message;
    }

}