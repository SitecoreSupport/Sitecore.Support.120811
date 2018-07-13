namespace Sitecore.Support.Shell.Applications.WebEdit.Commands
{
  using Sitecore;
  using Sitecore.Data;
  using Sitecore.Data.Items;
  using Sitecore.Diagnostics;
  using Sitecore.Shell.Applications.WebEdit.Commands;
  using Sitecore.Shell.Framework.Commands;
  using Sitecore.Text;
  using Sitecore.Web.UI.Sheer;
  using System;
  using System.Collections.Generic;
  using System.Collections.Specialized;
  using System.Web;
  using System.Web.UI;

  [Serializable]
  public class FieldEditor : WebEditCommand
  {
    public override void Execute(CommandContext context)
    {
      Assert.ArgumentNotNull(context, "context");
      if (context.Items.Length >= 1)
      {
        ClientPipelineArgs args = new ClientPipelineArgs(context.Parameters);
        args.Parameters.Add("uri", context.Items[0].Uri.ToString());
        Context.ClientPage.Start(this, "StartFieldEditor", args);
      }
    }

    protected PageEditFieldEditorOptions GetOptions(ClientPipelineArgs args, NameValueCollection form)
    {
      List<FieldDescriptor> fields = new List<FieldDescriptor>();
      Item item = Database.GetItem(ItemUri.Parse(args.Parameters["uri"]));
      Assert.IsNotNull(item, "item");
      string text1 = args.Parameters["fields"];
      Assert.IsNotNullOrEmpty(text1, "Field Editor command expects 'fields' parameter");
      string str = args.Parameters["command"];
      Assert.IsNotNullOrEmpty(str, "Field Editor command expects 'command' parameter");
      Item item2 = Client.CoreDatabase.GetItem(str);
      Assert.IsNotNull(item2, "command item");
      foreach (string str2 in new ListString(text1))
      {
        if (item.Fields[str2] != null)
        {
          fields.Add(new FieldDescriptor(item, str2));
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
      if (((context.Items.Length != 0) && (context.Items[0] != null)) && !context.Items[0].Access.CanWrite())
      {
        return CommandState.Disabled;
      }
      return CommandState.Enabled;
    }

    protected void StartFieldEditor(ClientPipelineArgs args)
    {
      HttpContext current = HttpContext.Current;
      if (current != null)
      {
        Page handler = current.Handler as Page;
        if (handler != null)
        {
          NameValueCollection form = handler.Request.Form;
          if (form != null)
          {
            if (!args.IsPostBack)
            {
              SheerResponse.ShowModalDialog(this.GetOptions(args, form).ToUrlString().ToString(), "720", "520", string.Empty, true);
              args.WaitForPostBack();
            }
            else if (args.HasResult)
            {
              PageEditFieldEditorOptions.Parse(args.Result).SetPageEditorFieldValues();
            }
          }
        }
      }
    }
  }
}
