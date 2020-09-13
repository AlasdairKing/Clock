/// <summary>
/// Contains I18Nisation code. C# equivalent of modI18N.vb in WebbIE4
/// Updated 27 Jan 2013
/// Updated 4 Feb 2013 to remove "contents" node from XML file.
/// Updated 16 June 2013 to make class "public" (so can be used on different forms) and remove
/// dependency on VisualBasic reference and add some documentation.
/// </summary>
public class I18N
{
    private System.Xml.XmlDocument _applicationXML;
    private string _languageCode;
    private bool _initialised;
    private bool _applicationXMLFound = false;
    private System.Xml.XmlDocument _commonXML;
    private bool _commonXMLFound = false;

    private bool _debug = false;
    
    //So for general string use we need a simple "GetText()" function that returns either (1) the translation or
    //(2) the original, which should be English so the code is readable.
    //   Is it in Application translations?
    //   If not, is it in Common translations?
    //   If not found, return original string.
    //For translating forms we need to do each control. 
    //FIRST for the .Text property:
    //   1 Is it one of the controls where we don't translate the .Text property? If yes, stop.
    //   2 Does it have a .Text property? Yes:
    //       3 If it has a tag, try tag.Text in the Application translations
    //       4 If this is not found, or there is no tag, try formName.controlName.Text in the Application translations
    //       5 If this is not found, then try controlName.Text in Common translations
    //       6 If this is not found, then leave the control unchanged (should stay in English)
    //SECOND for the .AccessibleName property:
    //   1 Does it have an .AccessibleName property? If no, stop.
    //   2 Is the .AccessibleName property ""? If yes, stop.
    //   3 Try formName.controlName.AccessibleName in the Application translations
    //   4 If this is not found, then leave the control unchanged. 

    //Use cases
    //   A simple string.
    //   A string containing quotation marks (get converted to single quotation marks)
    //   A string containing an ampersand.
    //   A control that has a .Text property but is not converted (ComboBox)
    //   A control that is set to not convert (WebBrowser) 
    //   A control that has an AccessibleName in addition to a .Text property and not being converted.
    //   A control that has a tag.
    //   A control that is not in the application language file but the common language file.


    /// <summary>
    /// Call this to make all successful translations display "xx" - that is, the language
    /// code is set to "xx", so that's what'll be found from the language files.
    /// </summary>
    public void SetDebug()
    {
        _initialised = false;
        Initialise();
        _languageCode = "xx";
        _debug = true;
    }

    /// <summary>
    /// Writes string s to a log file on the desktop.
    /// </summary>
    /// <param name="s"></param>
    private void WriteDebug(string s)
    {
        System.IO.StreamWriter sw = new System.IO.StreamWriter(System.Environment.GetFolderPath(System.Environment.SpecialFolder.DesktopDirectory) + "\\I18N Log.log", true, System.Text.Encoding.UTF8);
        sw.WriteLine(s);
        sw.Close();
        System.Windows.Forms.Application.DoEvents();
    }

    private void Initialise()
    {
        if (_initialised)
        {
            //Already loaded.
        }
        else
        {
            //Load language-specific file.
            _applicationXML = new System.Xml.XmlDocument();
            string applicationLanguagePath = System.Windows.Forms.Application.ExecutablePath;
            applicationLanguagePath = System.IO.Path.GetDirectoryName(applicationLanguagePath) + "\\" + System.IO.Path.GetFileNameWithoutExtension(applicationLanguagePath);
            applicationLanguagePath = applicationLanguagePath + ".Language.xml";
            if (System.IO.File.Exists(applicationLanguagePath))
            {
                try
                {
                    _applicationXML.Load(applicationLanguagePath);
                    _applicationXMLFound = true;
                }
                catch (System.Exception e)
                {
                    throw new System.Exception("Error loading the application language XML file, which was determined to be \"" + applicationLanguagePath + "\" The error returned was \"" + e.Message + "\"");
                }
            }
            else
            {
                //No language file!
                _applicationXMLFound = false;
                
            }

            //Load common file.
            _commonXML = new System.Xml.XmlDocument();
            string commonLanguagePath = System.Windows.Forms.Application.StartupPath + "\\Common.Language.xml";
            if (System.IO.File.Exists(commonLanguagePath))
            {
                try
                {
                    _commonXML.Load(commonLanguagePath);
                    _commonXMLFound = true;
                }
                catch (System.Exception e)
                {
                    throw new System.Exception("Error loading the application language XML file, which was determined to be \"" + applicationLanguagePath + "\" The error returned was \"" + e.Message + "\"");
                }
            }
            else
            {
                //No common language file!
                _commonXMLFound = false;
            }

            //Load the locale information.
            try
            {
                System.Globalization.CultureInfo ci = new System.Globalization.CultureInfo(System.Globalization.CultureInfo.CurrentUICulture.LCID);
                // Thread.CurrentThread.CurrentCulture.LCID);
                _languageCode = ci.TwoLetterISOLanguageName.ToLowerInvariant();
                //en or pt or fr or whatever.
            }
            catch
            {
                _languageCode = "en";
            }
            _initialised = true;
        }
    }

    /// <summary>
    /// Returns the current language code, e.g. "fr" or "en"
    /// </summary>
    /// <returns></returns>
    public string GetLanguage()
    {
        Initialise();
        return _languageCode;
    }


    /// <summary>
    /// Displays the help file, an RTF document found in the local folder, using WordPad (Write)
    /// </summary>
    public void ShowHelp()
    {
        string pathBase = System.Windows.Forms.Application.ExecutablePath;
        pathBase = System.IO.Path.GetDirectoryName(pathBase) + "\\" + System.IO.Path.GetFileNameWithoutExtension(pathBase);
        string path = pathBase + ".Help-" + _languageCode + ".rtf";
        if (!System.IO.File.Exists(path))
        {
            if (_debug)
                WriteDebug("Did not find help file: " + path);
            path = pathBase + ".Help-en.rtf";
        }
        if (System.IO.File.Exists(path))
        {
            System.Diagnostics.Process proc = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = proc.StartInfo;
            startInfo.UseShellExecute = false;
            startInfo.FileName = "write";
            startInfo.Arguments = "\"" + path + "\"";
            proc.Start();
   
            //Microsoft.VisualBasic.Interaction.Shell("write \"" + path + "\"", Microsoft.VisualBasic.AppWinStyle.NormalFocus);
        }
    }

    /// <summary>
    /// Localizes everything on form f.
    /// </summary>
    /// <param name="f"></param>
    public void DoForm(System.Windows.Forms.Form f)
    {
        Initialise();
        string name = f.Name;
        f.Text = GetText(name + ".Text");
        foreach (System.Windows.Forms.Control c in f.Controls)
        {
            DoControl(c, f.Name);
        }
    }

    /// <summary>
    /// DoControl translates a control AND its children - usually the .Text value,
    /// but the .AccessibleName if there is one.
    /// </summary>
    /// <param name="c"></param>
    /// <param name="formName"></param>
    /// <remarks></remarks>
    private void DoControl(System.Windows.Forms.Control c, string formName)
    {
        //Handle .Text property
        DoControlText(c, formName);

        //Handle .AccessibleName property
        DoControlAccessibleName(c, formName);

        // Handle ToolStrip control - includes menus like the File menu. 
        if ((c) is System.Windows.Forms.ToolStrip)
        {
            //Tool strip. Need to process items.
            foreach (System.Windows.Forms.ToolStripItem ti in ((System.Windows.Forms.ToolStrip)c).Items)
            {
                DoControlItem(ti, formName);
            }
        }

        //This child iteration handles things like Panel controls hosting TextBox 
        //controls. It doesn't handle things like ToolstripMenuItem controls in MenuStrips. See the 
        //code above for that.
        if (c.HasChildren)
        {
            foreach (System.Windows.Forms.Control cChild in c.Controls)
            {
                DoControl(cChild, formName);
            }
        }
    }

    private void DoControlText(System.Windows.Forms.Control c, string formName)
    {
        bool doText = false;
        //Some controls we do not look up: notably TextBox
        if ((c) is System.Windows.Forms.TextBox)
        {
            doText = false;
        }
        else if ((c) is System.Windows.Forms.RichTextBox)
        {
            doText = false;
        }
        else if ((c) is System.Windows.Forms.ComboBox)
        {
            doText = false;
        }
        else if ((c) is System.Windows.Forms.ListBox)
        {
            doText = false;
        }
        else if ((c) is System.Windows.Forms.MenuStrip)
        {
            doText = false;
        }
        else if ((c) is System.Windows.Forms.WebBrowser)
        {
            doText = false;
        }
        else if ((c) is System.Windows.Forms.ToolStrip)
        {
            doText = false;
        }
        else if ((c) is System.Windows.Forms.Panel)
        {
            doText = false;
        }
        else if ((c) is System.Windows.Forms.PictureBox)
        {
            doText = false;
        }
        else if ((c) is System.Windows.Forms.StatusBar)
        {
            doText = false;
        }
        else if ((c.Text == null))
        {
            doText = false;
        }
        else
        {
            doText = true;
        }
        if (doText)
        {
            bool useTag = false;
            if (c.Tag == null)
            {
                useTag = false;
            }
            else if (string.IsNullOrEmpty(c.Tag.ToString()))
            {
                useTag = false;
            }
            else
            {
                useTag = true;
            }
            string key = null;
            if (useTag)
            {
                key = c.Tag.ToString();
            }
            else
            {
                key = formName + "." + c.Name;
            }

            string textKey = key + ".Text";
            string text = GetText(textKey);
            if (text != textKey)
            {
                //Found an entry.
                c.Text = text;
            }
            else
            {
                //Failed to find anything when using the tag or the full formName.controlName
                //Try falling back now to our Common file, which should save me having to 
                //duplicate lots and lots of entries.
                textKey = c.Name + ".Text";
                text = GetText(textKey);
                if (text != textKey)
                {
                    c.Text = text;
                }
            }
        }
    }

    private void DoControlAccessibleName(System.Windows.Forms.Control c, string formName)
    {
        if (c.AccessibleName == null)
        {
        }
        else if (c.AccessibleName.Length == 0)
        {
        }
        else
        {
            string key = formName + "." + c.Name;
            string accKey = key + ".AccessibleName";
            string accName = GetText(accKey);
            if (accName != accKey)
            {
                c.AccessibleName = accName;
            }
        }
    }

    private void DoControlItem(System.Windows.Forms.ToolStripItem mi, string formName)
    {
        //Notice no .tag support. That's because in WebbIE there are lots of bookmark menu items
        //that have url tags. Don't want to I18N them. So skip tags for menus for now.
        string key = null;
        bool doText = true;
        if (mi.Name.Length == 0)
        {
            //No name: like, a menu element that is a divider, or empty - like the favorites in 
            //the WebbIE favorites menu.
            key = "";
            doText = false;
        }
        else
        {
            key = formName + "." + mi.Name;
        }
        if (doText)
        {
            string textKey = key + ".Text";
            string text = GetText(textKey);
            if (text != textKey)
            {
                mi.Text = text;
            }
            else
            {
                //Failed to find anything when using the tag or the full formName.controlName
                //Try falling back now to our Common file, which should save me having to 
                //duplicate lots and lots of entries.
                textKey = mi.Name + ".Text";
                text = GetText(textKey);
                if (text != textKey)
                {
                    mi.Text = text;
                }
            }
        }
        //Now do drop-down items.
        if ((mi) is System.Windows.Forms.ToolStripMenuItem & !((mi) is System.Windows.Forms.ToolStripSeparator))
        {
            foreach (object miChild in ((System.Windows.Forms.ToolStripMenuItem)mi).DropDownItems)
            {
                if ((miChild) is System.Windows.Forms.ToolStripSeparator)
                {
                }
                else
                {
                    DoControlItem((System.Windows.Forms.ToolStripMenuItem)miChild, formName);
                }
            }
        }
    }

    /// <summary>
    /// Returns the internationalised version of the string provided according to the current
    /// language (and the availability of the translation.)
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    /// <remarks>
    /// If there is no "item" node in AssemblyName.Language.xml that has a "key" node
    /// containing the argument text then the argument text is returned. This means that
    /// if the code calls GetText("Hello world") and no translation is provided then then
    /// function will return "Hello world". On the assumption that calling code is English
    /// there is therefore an implicit English default.
    /// If the "item" node that matches the argument text has a "leaveBlank" child then
    /// the empty string is returned.
    /// </remarks>
    public string GetText(string text)
    {
        Initialise();
        string key = null;
        key = text.Replace("\"", "'");
        System.Xml.XmlNode n = null;
        if (_applicationXMLFound)
        {
            n = _applicationXML.DocumentElement.SelectSingleNode("item[key=\"" + key + "\"]");
        }
        if (n == null)
        {
            //Nothing found in the application translation file: try our common language file.
            if (_commonXMLFound)
            {
                n = _commonXML.DocumentElement.SelectSingleNode("item[key=\"" + key + "\"]");
            }
            if (n == null)
            {
                if (_debug)
                {
                    WriteDebug("<item><key>" + key + "</key><content language=\"en\">" + text + "</content><content language=\"xx\">Xxxxx</content></item>");
                }
                return text;
            }
            else
            {
                System.Xml.XmlNode item = n.SelectSingleNode("content[@language=\"" + _languageCode + "\"]");
                if (item == null)
                {
                    return text;
                }
                else
                {
                    return item.InnerText;
                }
            }
        }
        else
        {
            if (n.SelectSingleNode("leaveBlank") == null)
            {
                System.Xml.XmlNode item = n.SelectSingleNode("content[@language=\"" + _languageCode + "\"]");
                if (item == null)
                {
                    return text;
                }
                else
                {
                    return item.InnerText;
                }
            }
            else
            {
                return "";
            }
        }
    }

} // end of class