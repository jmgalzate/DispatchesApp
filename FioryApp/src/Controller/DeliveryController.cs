using FioryApp.Entity;
using FioryApp.Service;

namespace FioryApp.Controller;

public class DeliveryController
{
    private OrderEntity orderObj { get; set; }
    public OrderEntity dispatchObj { get; set; }
    public int orderNumber { get; set; }

    public int totalProductsToScan { get; private set; } = 0;
    public int totalProductsScanned { get; private set; } = 0;
    public List<ProductEntity> productsOrder { get; private set; }
    public List<OrderProduct> productsDispatch { get; set; }


    private readonly SessionService _sessionService = new();

    /**
     * TODO: create the methods for handling the user request in the interface and the business logic.
     * - [x] Create the method for setting the products order.
     * - [x] Create the method for setting the products dispatch.
     * - [ ] Create the method for update the dispatch with the new list of products
     */

    public DeliveryController()
    {
    }

    public void SetDeliveryController(OrderEntity contapymeOrder, int order)
    {
        orderNumber = order;

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
        totalProductsToScan = productsOrder.Count;
    }

    public string SetProductsDispatched(string targetBarcode)
    {

        ProductEntity foundProduct =
            _sessionService.productsList.FirstOrDefault(product => product.barcode == targetBarcode);

        if (foundProduct != null)
        {
            // Check if the product exists in the Order
            ProductEntity foundOrderProduct =
                productsOrder.FirstOrDefault(product => product.code == foundProduct.code);
            if (foundOrderProduct != null)
            {
                // Check if the product exists in the Dispatch
                OrderProduct foundDispatchProduct =
                    productsDispatch.FirstOrDefault(product => product.irecurso == foundProduct.code);
                if (foundDispatchProduct != null)
                {
                    // If the product exists in the Dispatch then sum 1 to quantity
                    foundDispatchProduct.qrecurso++;
                    totalProductsScanned++;
                    return targetBarcode + "producto sumado";
                }
                else
                {
                    // If the product does not exist in the Dispatch then add the product to Dispatch

                    /**
                     * TODO:
                     * - [ ] Compare the FoundProduct with the productsOrder and add in ProductsToDispatch all the attributes
                     * - [ ] Calculate the new mvrtotal for the product
                     *
                     *   int nRecurso = x.quantity;
                        decimal vlr = nRecurso * y.mprecio;
                        decimal dcto = y.qporcdescuento / 100;
                        decimal vlrDescuento = dcto * vlr;
                        mvrtotal = (vlr - vlrDescuento),
                     */

                    OrderProduct newDispatchProduct = new()
                    {
                        irecurso = foundProduct.code,
                        qrecurso = 1

                    };
                    
                    productsDispatch.Add(newDispatchProduct);
                    totalProductsScanned++;
                    return "producto agregado";
                }
            }
            else
            {
                // If the product does not exist in the Order then return a message
                return targetBarcode + "producto no requerido";
            }
        }
        else
        {
            // If the product does not exist in Master Data then return a message
            return targetBarcode + "producto no encontrado";
        }
    }
}