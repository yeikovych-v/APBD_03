using APBD_03.controller;
using APBD_03.model;
using APBD_03.model.containers;
using APBD_03.model.repository;
using APBD_03.repository;
using APBD_03.service;

public class Program
{
    public static void Main(string[] args)
    {
        InitDb();
        ConsoleCommandsController.PrintSeparator();
        ConsoleCommandsController.InitConsoleInterface();
    }

    private static void InitDb()
    {
        ShipRepository.AddAll(MockService.MockShips());
        ContainerRepository.AddAll(MockService.MockContainers());
    }
}