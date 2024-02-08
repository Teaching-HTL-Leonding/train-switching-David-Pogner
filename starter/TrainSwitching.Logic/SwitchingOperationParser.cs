using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices;
using Microsoft.VisualBasic;

namespace TrainSwitching.Logic;

public static class SwitchingOperationParser
{
    /// <summary>
    /// Parses a line of input into a <see cref="SwitchingOperation"/>.
    /// </summary>
    /// <param name="inputLine">Line to parse. See readme.md for details</param>
    /// <returns>The parsed switching operation</returns>
    public static SwitchingOperation Parse(string inputLine)
    {
        var parts = inputLine.Split(' ');
        var trackNumber = TrackNumber(parts);
        var operationType = OperationType(parts[3]);
        var direction = Direction(parts[^1]);
        int? numberOfWagons = null;
        var operation = new SwitchingOperation();
        var wagonType = WagonType(inputLine);
        
        if (operationType == Constants.OPERATION_REMOVE)
        {
            numberOfWagons = NumberOfWagons(inputLine);
        }


        operation.TrackNumber = trackNumber;
        operation.OperationType = operationType;
        operation.Direction = direction;
        operation.WagonType = wagonType;
        operation.NumberOfWagons = numberOfWagons;

        return operation;
    }

    public static int TrackNumber(string[] parts)
    {
        var trackNumber = int.Parse(parts[2].TrimEnd(','));
        return trackNumber;
    }

    public static int OperationType(string operation)
    {
        switch (operation.ToLower())
        {
            case "add":
                return Constants.OPERATION_ADD;
            case "remove":
                return Constants.OPERATION_REMOVE;
            case "train":
                return Constants.OPERATION_TRAIN_LEAVE;
            default:
            throw new ArgumentException("Invalid operation type");
        }
    }

    public static int Direction(string operation)
    {
        switch (operation.ToLower())
        {
            case "east":
                return Constants.DIRECTION_EAST;
            case "west":
                return Constants.DIRECTION_WEST;
            default:
                throw new ArgumentException("Invalid direction");
        }
    }

    public static int? WagonType(string inputLine)
    {
        if (inputLine.Contains("Passenger Wagon")) { return Constants.WAGON_TYPE_PASSENGER; }
        if (inputLine.Contains("Locomotive")) { return Constants.WAGON_TYPE_LOCOMOTIVE; }
        if (inputLine.Contains("Freight Wagon")) { return Constants.WAGON_TYPE_FREIGHT; }
        if (inputLine.Contains("Car Transport Wagon")) { return Constants.WAGON_TYPE_CAR_TRANSPORT; }
        if(inputLine.Contains("train") || NumberOfWagons(inputLine) > 0) { return null; }
        throw new ArgumentException("Invalid Wagon Type");;
    }

    public static int? NumberOfWagons(string inputLine)
    {
        var commaIndex = inputLine.IndexOf(',');
        var inputAfterComma = inputLine.Substring(commaIndex + 1).Trim();

        for (int i = 0; i < inputAfterComma.Length; i++)
        {
            if (char.IsDigit(inputAfterComma[i]))
            {
                return int.Parse(inputAfterComma[i].ToString());
            }
        }

        return null;
    }
}