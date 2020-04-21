using System;
using DesignPatternsTest.BehavioralPatterns.Strategy.Implementations;
using DesignPatternsTest.CreationalPatterns.AbstractFactory.Implementations;
using DesignPatternsTest.CreationalPatterns.AbstractFactory.Interfaces;
using DesignPatternsTest.CreationalPatterns.FactoryMethod.Implementations;

namespace DesignPatternsTest
{
    class Program
    {
        static void Main(string[] args)
        {
            //FactoryMethodDemo();
            //AbstractFactoryDemo();
            StrategyDemo();
            Console.ReadKey();
        }

        public static void FactoryMethodDemo()
        {
            var winButtonCreator = new WinButtonCreator();
            var winButton = winButtonCreator.CreateButton();
            winButton.Draw();
            var osxButtonCreator = new OsxButtonCreator();
            var osxButton = osxButtonCreator.CreateButton();
            osxButton.Draw();
        }

        public static void AbstractFactoryDemo()
        {
            var key = Console.ReadLine();
            IGUIFactory factory;
            switch (key)
            {
                case "W":
                    factory = new WinFactory();
                    break;
                case "O":
                    factory = new OsxFactory();
                    break;
                default:
                    factory = new WinFactory();
                    break;
            }

            CreateWindowWithTwoButtonsAndProgressBar(factory);
        }

        public static void StrategyDemo()
        {
            var hybridCar = new HybridCar(new PetrolEngine());
            hybridCar.Move();
            hybridCar.Strategy = new ElectricEngine();
            hybridCar.Move();
        }

        private static void CreateWindowWithTwoButtonsAndProgressBar(IGUIFactory factory)
        {
            var button1 = factory.CreateButton();
            var button2 = factory.CreateButton();
            var progressBar = factory.CreateProgressBar();
            button1.Draw();
            button2.Draw();
            progressBar.Draw();
            progressBar.SetProgress(45);
        }
    }
}
