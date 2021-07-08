# Webshop management application
> It's an application for managing a webshop

## General info
The application is designed to manage an online shop. It allows you to manage employees, customers, product categories and products. Thanks to different accessibility levels you can traverse an application as an administrator, employee, customer and guest. The administrator panel allows you to manage employees and customers. Through an employee panel, you can manage products, categories and customer orders. As a registered and signed-in customer you have access to the cart and orders you made. Guest is able to browse products filtered by specific categories and see product details.

**The application was developed based on Clean Architecture pattern.**

## Technologies
* .NET 5
* Entity Framework Core 5
* AutoMapper
* XUnit
* Moq
* Fluent Validation
* MSSQL
* Bootstrap 4
* LINQ
* ASP.NET Core
* HTML
* CSS
* Dependency Injection
* WebApi

## Features
### Administrator panel:
* Employee Management - Search employees, employee details, add, edit remove employee.
* Customer Management - Search customers, customer details including their shopping cart and order.
### Employee Panel:
* Product Category Management - Search product categories, category details, add, edit and remove category.
* Product Management - Search products, product details, add, edit and remove product.
* Order Management - Search orders, order details and delete order.
### Customer functionalities
* Browsing products, viewing product details, adding to shopping cart.
* Shopping Cart - Displays customer cart content, increase and decrease product quantity, remove items from cart.
* Order History - Search orders and view order details.