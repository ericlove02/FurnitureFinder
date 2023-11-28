using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using EnhancedTouch = UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.UI;
using System;
using TMPro;
using UnityEngine.Networking;
using Newtonsoft.Json;
using System.Linq;

public class SelectedFurniture
{
    public static string clickedFurnitureName = "Default";
    public static int clickedFurnitureId = 1;
    public static string previousPage = "None";
    public static List<ScrollViewManager.FurnitureData> furnitureData;
    public static string furniturePurchaseLink = "";
    public static Sprite[] furniturePics; 
}