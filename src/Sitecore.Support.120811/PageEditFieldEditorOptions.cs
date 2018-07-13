namespace Sitecore.Support.Shell.Applications.WebEdit
{
  using Sitecore;
  using Sitecore.Data;
  using Sitecore.Data.Items;
  using Sitecore.Diagnostics;
  using Sitecore.ExperienceEditor.Utils;
  using Sitecore.Shell.Applications.ContentManager;
  using Sitecore.Shell.Applications.WebEdit.Commands;
  using System;
  using System.Collections.Generic;
  using System.Collections.Specialized;
  using System.Linq;
  using Sitecore.Shell.Applications.WebEdit;
  using System.Reflection;

  public class PageEditFieldEditorOptions : FieldEditorOptions
  {
    private PageEditFieldEditorOptions(FieldEditorOptions innerOptions) : base(innerOptions.Fields)
    {
    }

    public PageEditFieldEditorOptions(NameValueCollection form, IEnumerable<FieldDescriptor> fields) : base(fields)
    {
      this.LoadPageEditorFieldValues(form);
    }

    public void LoadPageEditorFieldValues(NameValueCollection form)
    {
      Assert.ArgumentNotNull(form, "form");
      object[] parameters = new object[] { form };
      IEnumerable<PageEditorField> fields = Type.GetType("Sitecore.ExperienceEditor.Utils.WebEditUtility, Sitecore.ExperienceEditor").GetMethod("GetFields", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, parameters) as IEnumerable<PageEditorField>;
      foreach (FieldDescriptor descriptor in base.Fields)
      {
        //FieldDescriptor descriptor = descriptor;
        PageEditorField field = fields.FirstOrDefault<PageEditorField>(f => (f.ItemID == descriptor.ItemUri.ItemID) && (f.FieldID == descriptor.FieldID));
        if (field != null)
        {
          descriptor.Value = field.Value;
        }
      }
    }

    public static PageEditFieldEditorOptions Parse(string handle) =>
        new PageEditFieldEditorOptions(FieldEditorOptions.Parse(handle));

    public void SetPageEditorFieldValues()
    {
      WebEditResponse.EditFrameUpdateStart();
      foreach (FieldDescriptor descriptor in base.Fields)
      {
        Item itemNotNull = Client.GetItemNotNull(descriptor.ItemUri);
        WebEditResponse.SetFieldValue(itemNotNull, itemNotNull.Fields[descriptor.FieldID], descriptor.Value);
      }
      WebEditResponse.EditFrameUpdateEnd();
    }
  }
}
