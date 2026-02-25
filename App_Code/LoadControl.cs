using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;


    public static class LoadControlExtension
    {
        /// <summary>
        /// Loads a user control with a constructor with a signature matching the supplied params
        /// Control must implement a blank default constructor as well as the custom one or we will error
        /// </summary>
        /// <param name="controlPath">Path to the user control</param>
        /// <param name="constructorParams">Parameters for the constructor</param>
        /// <returns></returns>
        public static UserControl LoadControl(this TemplateControl templateControl, string controlPath, params object[] constructorParams)
        {
            // Load the control
            var control = templateControl.LoadControl(controlPath) as UserControl;

            // Get the types for the passed parameters
            Type[] paramTypes = new Type[constructorParams.Length];
            for (int paramLoop = 0; paramLoop < constructorParams.Length; paramLoop++)
                paramTypes[paramLoop] = constructorParams[paramLoop].GetType();

            // Get the constructor that matches our signature
            var constructor = control.GetType().BaseType.GetConstructor(paramTypes);

            // Call the constructor if we found it, otherwise throw
            if (constructor == null)
            {
                throw new ArgumentException("Required constructor signature not found.");
            }
            else
            {
                constructor.Invoke(control, constructorParams);
            }

            return control;
        }

        public static UserControl LoadControl(this TemplateControl templateControl, string controlPath)
        {
            // Load the control
            var control = templateControl.LoadControl(controlPath) as UserControl;
            object[] constructorParams = new object[0];

            // Get the types for the passed parameters
            Type[] paramTypes = new Type[0];
            for (int paramLoop = 0; paramLoop < constructorParams.Length; paramLoop++)
                paramTypes[paramLoop] = constructorParams[paramLoop].GetType();

            // Get the constructor that matches our signature
            var constructor = control.GetType().BaseType.GetConstructor(paramTypes);

            // Call the constructor if we found it, otherwise throw
            if (constructor == null)
            {
                throw new ArgumentException("Required constructor signature not found.");
            }
            else
            {
                constructor.Invoke(control, constructorParams);
            }

            return control;
        }
    }

    public class ce
    {

        public static void SetEditorCSS(CuteEditor.Editor ce)
        {
            string TemplateItemList = "CssClass,Bold,Italic,Underline,|,JustifyLeft,JustifyCenter,JustifyRight,JustifyFull,|,InsertOrderedList,InsertUnorderedList,|,InsertLink,InsertImage,InsertFlash,InsertMedia,InsertDocument,|,RemoveFormat,CleanCode";
            ce.TemplateItemList = TemplateItemList;

            ce.ThemeType = CuteEditor.ThemeType.Office2007;
            ce.ResizeMode = CuteEditor.EditorResizeMode.None;
            ce.Height = 200;
            
            ce.EditorWysiwygModeCss = "/css/main.css";
            ce.PreviewModeCss = "/css/main.css";

            CuteEditor.ToolControl toolctrl = ce.ToolControls["CssClass"];

            if (toolctrl != null)
            {
                CuteEditor.RichDropDownList dropdown = (CuteEditor.RichDropDownList)toolctrl.Control;
                //the first item is the caption
                CuteEditor.RichListItem richitem = dropdown.Items[0];
                //clear the items from configuration files
                dropdown.Items.Clear();
                //add the caption
                dropdown.Items.Add(richitem);
                //add value only

                dropdown.Items.Add("<span style='font-family:Arial,Sans-Serif;font-size:13px;color:#000000;'>Main Body</span>", "Main Body", "bodytext");
                dropdown.Items.Add("<span style='font-family:Arial,Sans-Serif;font-size:13px;color:#000;'>White Body (white)</span>", "White Body", "bodytext_white");
                dropdown.Items.Add("<span style='font-family:Arial,Sans-Serif;font-size:14px;font-weight:bold;color:#b13e43;'>SubTitle</span>", "SubTitle", "subtitle");
                dropdown.Items.Add("<span style='font-family:Arial;font-size:14px;font-weight:bold;color:#000;'>White SubTitle (white)</span>", "White SubTitle", "subtitle_white");
                dropdown.Items.Add("<span style='font-family:Arial,Sans-Serif;font-size:17px;font-weight:bold;color:#b13e43;'>Title</span>", "Title", "title");
                dropdown.Items.Add("<span style='font-family:Arial,Sans-Serif;font-size:24px;color:#000000;'>Header</span>", "Header", "header");
                dropdown.Items.Add("<span style='font-family:Arial,Sans-Serif;font-size:24px;color:#000;'>White Header (white)</span>", "White Header", "header_white");
            }
        }
    }