namespace Sitecore.Support.Shell.Applications.WebEdit.Commands
{
  using Sitecore;
  using Sitecore.Data;
  using Sitecore.Data.Items;
  using Sitecore.Diagnostics;
  using Sitecore.Shell.Applications.WebEdit;
  using Sitecore.Shell.Framework.Commands;
  using Sitecore.Text;
  using Sitecore.Web.UI.Sheer;
  using System;
  using System.Collections.Generic;
  using System.Collections.Specialized;
  using Sitecore.Shell.Applications.WebEdit.Commands;

[Serializable]
  public class FieldEditor : FieldEditorCommand
  {
    protected override PageEditFieldEditorOptions GetOptions(ClientPipelineArgs args, NameValueCollection form)
    {
      List<FieldDescriptor> fields = new List<FieldDescriptor>();
      Item item = Database.GetItem(ItemUri.Parse(args.Parameters["uri"]));
      Assert.IsNotNull(item, "item");
      string str = args.Parameters["fields"];
      Assert.IsNotNullOrEmpty(str, "Field Editor command expects 'fields' parameter");
      string str2 = args.Parameters["command"];
      Assert.IsNotNullOrEmpty(str2, "Field Editor command expects 'command' parameter");
      Item item2 = Client.CoreDatabase.GetItem(str2);
      Assert.IsNotNull(item2, "command item");
      foreach (string str3 in new ListString(str))
      {
        if (item.Fields[str3] != null)
        {
          fields.Add(new FieldDescriptor(item, str3));
        }
      }
      return new PageEditFieldEditorOptions(form, fields)
      {
        Title = item2["Title"],
        Icon = item2["Icon"]
      };
    }

    public override CommandState QueryState(CommandContext context)
    {
      Assert.ArgumentNotNull(context, "context");
      if (((context.Items.Length > 0) && (context.Items[0] != null)) && !context.Items[0].Access.CanWrite())
      {
        return CommandState.Disabled;
      }
      return CommandState.Enabled;
    }
  }
}
