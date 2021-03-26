using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using UnityEngine.UI;
public class Background : MonoBehaviour
{
    public RawImage imageRenderer;
    public RectTransform recTransform;
    public void Start()
    {
        //Get screen resolution
        Resolution resolution = Screen.currentResolution;
        recTransform = this.GetComponent<RectTransform>();
        imageRenderer = this.GetComponent<RawImage>();
        //Set the raw image inside the canvas to the screen resolution
        recTransform.sizeDelta = new Vector2(resolution.width,resolution.height);
        StartCoroutine("SetDesktopImage");
    }
    IEnumerator SetDesktopImage()
    {
        //Load desktop image from windows cache using WWW
        WWW myWWW = new WWW("file://" + GetDesktopImagePath());
        yield return myWWW;
        imageRenderer.texture = myWWW.texture;
    }
    public String GetDesktopImagePath()
    {
        var myString = "";
        //Get the appdata folder where windows stores its background cache
        var folder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)
            + "\\Microsoft\\Windows\\Themes\\CachedFiles";
        DirectoryInfo d = new DirectoryInfo(folder);
        var images = d.GetFiles("*.jpg");
        /*Windows stores multiple sizes, we want the highest quality image,
         * so lets look for the biggest size.
         */
        var biggest = images[0];
        foreach (var file in images)
        {
            if(file.Length > biggest.Length)
            {
                biggest = file;
            }
        }
        myString = folder + "\\" + biggest.Name;
        //Finally change all backslashes to forward slashes so it will work with WWW
        myString.Replace('\\', '/');
        return myString;
    }
}
