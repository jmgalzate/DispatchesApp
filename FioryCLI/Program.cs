using System;
using System.Reflection;

using FioryLibrary.Connections;
using FioryLibrary.Operations;
using FioryLibrary.Sales;

namespace Fiory;

class Fiory
{
    static void Main(string[] args)
    {
        var apiAgente = new Contapyme();
        var masterData = new Masterdata();

        apiAgente.setContapyme();

        bool workNewOrder;
        bool scanOperationControl;
        bool controlSaveOrder;

        Console.WriteLine($"***** FIORY *******");
        Console.WriteLine("Gestión de Pedidos");
        Console.WriteLine($"*******************");

        do // Start Order
        {

            Console.Write("Ingresa el pedido: ");
            apiAgente.OrderNumber = int.Parse(Console.ReadLine()!);
            apiAgente.process();

            var order = new Order();
            order.setOrderNumber(apiAgente.OrderNumber);
            order.setOrderDetails(apiAgente.load());

            var scanControl = new Scanned();
            scanControl.setListScannedProducts(order.products);

            Console.WriteLine();
            Console.WriteLine($"El pedido requiere {order.totalProductsToScan} productos para escanear");
            Console.WriteLine();
            Console.WriteLine("Si desea escanear menos productos de ENTER, únicamente se guardarán los productos que lleva escaneados hasta el momento.");
            Console.WriteLine();

            do // Scan control point 
            {
                do // Scan operation point
                {
                    Console.Write($"Ingrese el producto {scanControl.indexScanned + 1}/{order.totalProductsToScan}: ");
                    string stringItem = Console.ReadLine()!;

                    if (string.IsNullOrEmpty(stringItem))
                    {
                        break;
                    }
                    else
                    {
                        var sku = masterData.vlookup(stringItem);

                        if (string.IsNullOrEmpty(sku))
                        {
                            Console.WriteLine($"El código {stringItem} no está definido en la base de productos");
                        }
                        else
                        {
                            var existsInOrder = order.vlookup(sku);
                            if (existsInOrder == true)
                            {
                                scanControl.addProduct(sku, stringItem);
                                scanControl.indexScanned++;
                            }
                            else
                            {
                                Console.WriteLine($"¡OJO! El EAN {stringItem} no fue solicitado pedido {order.orderNumber}");
                            }
                        }
                    }

                } while (scanControl.indexScanned < order.totalProductsToScan);

                Console.WriteLine();
                Console.WriteLine("********************");
                Console.WriteLine("Productos escaneados: ");
                Console.WriteLine("CODIGO\t\tEAN\t\tCANTIDAD");
                var scanned = scanControl.scannedProducts;
                scanned.ForEach(x => Console.WriteLine($"{x.sku}\t{x.ean}\t{x.quantity}"));
                Console.WriteLine("********************");
                Console.WriteLine();

                if (scanControl.indexScanned < order.totalProductsToScan)
                {
                    Console.Write("¿Continuar escaneando? s/n: ");
                    scanOperationControl = (Console.ReadLine()!.ToLower() == "s") ? true : false;
                }
                else
                {
                    scanOperationControl = false;
                }

            } while (scanOperationControl == true);

            var dispatch = new Dispatch();
            dispatch.orderNumber = order.orderNumber;
            dispatch.setSkuDetails(scanControl.scannedProducts);
            dispatch.setListOfProducts(order.products);
            dispatch.setOrderDetails(order.orderJson!);
            Sale.exportReport(dispatch.orderNumber, dispatch.orderJson!, order.products, dispatch.products);

            Console.Write("¿Desea actualizar el pedido en Contapyme? s/n: ");
            controlSaveOrder = (Console.ReadLine()!.ToLower() == "s") ? true : false;
            if (controlSaveOrder == true) apiAgente.save(dispatch.orderJson!);

            apiAgente.unprocess();
            apiAgente.closeAgent();

            Console.Write("¿Otro pedido? s/n: ");
            workNewOrder = (Console.ReadLine()!.ToLower() == "s") ? true : false;

        } while (workNewOrder == true); //Close order

        Console.WriteLine();
        Console.WriteLine("¡Recuerda guardar los exportes en tus archivos como Respaldo!");
        Console.WriteLine("¡Hasta pronto! =)");
    }
}