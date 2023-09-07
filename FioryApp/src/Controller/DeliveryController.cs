using FioryApp.Entity;
using FioryApp.Service;

namespace FioryApp.Controller;

public class DeliveryController
{
    private OrderEntity orderObj { get; set; }
    public OrderEntity dispatchObj { get; private set; }

    public int totalProductsToScan { get; private set; }
    public int totalProductsScanned { get; private set; }
    public decimal efficiency { get; private set; }
    public List<ProductEntity> productsOrder { get; private set; }
    private List<OrderProduct> productsDispatch { get; set; }

    private readonly List<ProductEntity> _productList;

    public DeliveryController(SessionService sessionService)
    {
        _productList = sessionService.productsList;
    }

    public void SetDeliveryController(OrderEntity contapymeOrder, int order)
    {
        LoggerService.CreateLogFile().GetAwaiter().GetResult();
        LoggerService.Info("Scan Operation: the operation for scanning is started for order " + order);

        orderObj = contapymeOrder;
        ConfigFilesService.ExportFile(orderObj, "actual", order);
        
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

    public void SetDispatch(int order)
    {
        dispatchObj.listaproductos = productsDispatch;
        dispatchObj.encabezado!.iusuarioult = "WEBAPI";
        ConfigFilesService.ExportFile(dispatchObj, "nuevo", order);
        ConfigFilesService.ExportReport(dispatchObj.datosprincipales!.init, dispatchObj.encabezado!.fcreacion, order,
            efficiency, productsOrder);
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
            code = orderProduct.irecurso,
            quantity = 0,
            requested = orderProduct.qrecurso
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
                OrderProduct foundProductInOrder =
                    orderObj.listaproductos!.FirstOrDefault(product => product.irecurso == foundProduct.code);

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
                            qrecurso = 1,
                            mprecio = foundProductInOrder.mprecio,
                            qporcdescuento = foundProductInOrder.qporcdescuento,
                            qporciva = foundProductInOrder.qporciva,
                            mvrtotal = (foundProductInOrder.mprecio - (foundProductInOrder.mprecio * foundProductInOrder.qporcdescuento / 100) * 1),
                            valor1 = foundProductInOrder.valor1,
                            valor2 = foundProductInOrder.valor2,
                            valor3 = foundProductInOrder.valor3,
                            valor4 = foundProductInOrder.valor4,
                            qrecurso2 = foundProductInOrder.qrecurso2,
                        });
                        
                        message = "Producto agregado";
                        productsOrder[productsOrder.FindIndex(product => product.code == foundProduct.code)].quantity++;
                        totalProductsScanned++;
                        efficiency = Math.Round((decimal)totalProductsScanned / totalProductsToScan, 3);
                    }
                    else
                    {
                        int foundProductIndexRequested =
                            productsOrder.FindIndex(product => product.code == foundProduct.code);

                        if (productsOrder[foundProductIndexRequested].requested ==
                            productsOrder[foundProductIndexRequested].quantity)
                        {
                            message = "Producto ya completado";
                            return message;
                        }
                        else
                        {
                            int foundProductIndexDispatch =
                                productsDispatch.FindIndex(product => product.irecurso == foundProduct.code);

                            if (foundProductIndexDispatch >= 0)
                            {
                                productsDispatch[foundProductIndexDispatch].qrecurso =
                                    productsDispatch[foundProductIndexDispatch].qrecurso + 1;
                                decimal newPrice = productsDispatch[foundProductIndexDispatch].qrecurso *
                                                   productsDispatch[foundProductIndexDispatch]
                                                       .mprecio;

                                decimal discount = productsDispatch[foundProductIndexDispatch].qporcdescuento / 100;
                                productsDispatch[foundProductIndexDispatch].mvrtotal = newPrice - (newPrice * discount);
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
                                    qrecurso = 1,
                                    mprecio = foundProductInOrder.mprecio,
                                    qporcdescuento = foundProductInOrder.qporcdescuento,
                                    qporciva = foundProductInOrder.qporciva,
                                    mvrtotal = (foundProductInOrder.mprecio - (foundProductInOrder.mprecio * foundProductInOrder.qporcdescuento / 100) * 1),
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