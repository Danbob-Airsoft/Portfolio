using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

/// <summary>
///
/// Loads the inventory for the player from disk
///
/// Created by: Lewis Arnold
/// Edited by: Daniel Bailey
/// </summary>
/// 
public static class LoadInventory
{
    private static string inventoryFile = "InventoryState.txt";
    private static string FilePath = Application.dataPath + "/Resources/" + inventoryFile;
    public static int[] Load()
    {
        if (!File.Exists(FilePath))
        {
            CreateSaveFile();
        }

        int[] returnable = new int[3];
        int index = 0;
        StreamReader sr = File.OpenText(FilePath);
        while (!sr.EndOfStream)
        {
            Debug.Log("Got a line of text");
            String temp = sr.ReadLine();
            returnable[index] = int.Parse(temp);
            index++;
        }

        sr.Close();

        return returnable;
    }

    public static void Save(int[] inventoryState)
    {
        if (File.Exists(FilePath))
        {
            File.Delete(FilePath);
        }

        StreamWriter sw = CreateSaveFile(true);

        foreach (int state in inventoryState)
        {
            sw.WriteLine(state);
        }

        sw.Close();
    }

    private static StreamWriter CreateSaveFile(bool keepFileOpen = false)
    {
        Debug.Log("Created save file!");
        if(keepFileOpen)
            return File.CreateText(FilePath);

        StreamWriter sw = File.CreateText(FilePath);
        sw.Close();
        return null;
    }
}
