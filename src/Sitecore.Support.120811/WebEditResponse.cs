namespace Sitecore.Support.Shell.Applications.WebEdit.Commands
{
  using Sitecore.Data.Fields;
  using Sitecore.Data.Items;
  using Sitecore.Diagnostics;
  using Sitecore.Shell.Applications.WebEdit;
  using Sitecore.StringExtensions;
  using System;

  public static class WebEditResponse
  {
    internal static void EditFrameUpdateEnd()
    {
      Sitecore.Shell.Applications.WebEdit.WebEditResponse.Commands.Add("Sitecore.PageModes.ChromeManager.handleMessage('chrome:editframe:updateend');");
    }

    internal static void EditFrameUpdateStart()
    {
      Sitecore.Shell.Applications.WebEdit.WebEditResponse.Commands.Add("Sitecore.PageModes.ChromeManager.handleMessage('chrome:editframe:updateend');");
    }

    internal static void SetFieldValue(Item item, Field field, string value)
    {
      Assert.ArgumentNotNull(item, "item");
      Assert.ArgumentNotNull(field, "field");
      Assert.ArgumentNotNull(value, "value");
      value = value.Replace("\r", string.Empty).Replace("\n", string.Empty).Replace("\t", string.Empty);
      value = value.Replace("\"", "-,scDQuote,-").Replace("'", "-,scSQuote,-");
      object[] parameters = new object[] { item.ID.ToShortID().ToString(), item.Language.Name, item.Version.ToString(), item.Statistics.Revision };
      string str = "new Sitecore.ItemUri('{0}', '{1}', '{2}', '{3}')".FormatWith(parameters);
      object[] objArray2 = new object[] { str, field.ID.ToShortID().ToString(), value };
      string str2 = "Sitecore.WebEdit.setFieldValue({0}, '{1}', '{2}');".FormatWith(objArray2);
      Sitecore.Shell.Applications.WebEdit.WebEditResponse.Commands.Add(str2);
    }
  }
}
