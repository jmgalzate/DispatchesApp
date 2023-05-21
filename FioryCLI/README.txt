*********** FIORY Dispatch application ***********

This is not a comercial software created for helping to the Foiry's warehouse for dispatching their products. The solution was requested to automatizate a process they already had impletementd.

What did they are doing?
1. Open the UI application called Contapyme
2. Search by the order they should dispatch
3. Copy and paste the requested codes into an excel file
4. Scan the codes they will dispatch into the same excel file
5. Use a vlookup formula for compairing the requested and scanned products
6. Edit de order in Contapyme with the new quantity of products

They an excel file by month, and each sheet is a workday, so, the information is so sensible to be lost.

What they requeste for?
Remove the excel scteps creating an application that can:
- download the order information
- prepare the order to see how many products they should scan
- check if the scanned EAN exists in the products master data, and if it's requested by the order
- the possibility to modify the order by less quantity than expected in the order.
- save the new order
- modify the order directly in Contapyme

For doing it, the builded application do:
- create an API connection with Contapyme
- request the order information
- export the current order as a back up
- receives the scanner process and check if the EAN code exists in the products master data and in the previous order.
- allow to scan less products quantity
- modify and send the modification to the Contapyme API integration.