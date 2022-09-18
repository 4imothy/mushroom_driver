using UnityEngine;

public static class CarCosts
{

    private static int truckCost = 10000;
    private static int boatCost = 15000;
    private static int sportsCost = 30000;
    private static int miniCost = 50000;
    private static int beetleCost = 75000;
    private static int cheeseCost = 100000;
    private static int hoverCost = 150000;
    private static int mushroomCarCost = 200000;

    public static int cost(string carName)
    {
        switch (carName)
        {
            case "Mushroom":
                return mushroomCarCost;
            case "Truck":
                return truckCost;
            case "Cheese":
                return cheeseCost;
            case "Sports":
                return sportsCost;
            case "Hover":
                return hoverCost;
            case "Boat":
                return boatCost;
            case "Mini":
                return miniCost;
            case "Beetle":
                return beetleCost;
        }
        return 0;
    }

    public static string carName(string objName)
    {
        switch (objName)
        {
            case "Mushroom":
                return "Mushroom Car";
            case "Truck":
                return "Truck";
            case "Cheese":
                return "Cheese Car";
            case "Sports":
                return "Sports Car";
            case "Hover":
                return "Hover Car";
            case "Boat":
                return "Boat Car";
            case "Mini":
                return "Mini";
            case "Beetle":
                return "Beetle";
        }
        return "";
    }
}
