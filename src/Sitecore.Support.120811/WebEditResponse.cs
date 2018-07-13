namespace Sitecore.Support.Shell.Applications.WebEdit
{
  using Sitecore.Data.Fields;
  using Sitecore.Data.Items;
  using Sitecore.Diagnostics;
  using Sitecore.StringExtensions;
  using Sitecore.Web;
  using Sitecore.Web.UI.Sheer;
  using System;
  using System.Collections.Generic;

  public static class WebEditResponse
  {
    internal static void EditFrameUpdateEnd()
    {
      Commands.Add("Sitecore.PageModes.ChromeManager.handleMessage('chrome:editframe:updateend');");
    }

    internal static void EditFrameUpdateStart()
    {
      Commands.Add("Sitecore.PageModes.ChromeManager.handleMessage('chrome:editframe:updatestart');");
    }

    public static void Eval(string javascript)
    {
      Commands.Add(javascript);
    }

    public static void ExecuteCommands()
    {
      foreach (string str in Commands)
      {
        SheerResponse.Eval(string.Format("if(!Sitecore.PageModes) {{ window.parent.eval(\"{0}\"); }} else {{ window.eval(\"{0}\"); }}", str));
      }
    }

    public static void SetFieldValue(Item item, Field field, string value)
    {
      Assert.ArgumentNotNull(item, "item");
      Assert.ArgumentNotNull(field, "field");
      Assert.ArgumentNotNull(value, "value");
      value = value.Replace("\r", string.Empty).Replace("\n", string.Empty).Replace("\t", string.Empty);
      value = value.Replace("\"", "-,scDQuote,-").Replace("'", "-,scSQuote,-");
      string str = "new Sitecore.ItemUri('{0}', '{1}', '{2}', '{3}')".FormatWith(new object[] { item.ID.ToShortID().ToString(), item.Language.Name, item.Version.ToString(), item.Statistics.Revision });
      string str2 = "Sitecore.WebEdit.setFieldValue({0}, '{1}', '{2}');".FormatWith(new object[] { str, field.ID.ToShortID().ToString(), value });
      Commands.Add(str2);
    }

    public static ICollection<string> Commands
    {
      get
      {
        ICollection<string> itemsValue = WebUtil.GetItemsValue("WebEditResponse.Commands") as ICollection<string>;
        if (itemsValue == null)
        {
          WebUtil.SetItemsValue("WebEditResponse.Commands", new List<string>());
          itemsValue = WebUtil.GetItemsValue("WebEditResponse.Commands") as ICollection<string>;
        }
        return itemsValue;
      }
    }
  }
}
