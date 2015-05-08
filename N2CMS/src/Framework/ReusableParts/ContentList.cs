/*************************************************************************************************

Content List: Generic Model
Licensed to users of N2CMS under the terms of the Boost Software License

Copyright (c) 2013 Benjamin Herila <mailto:ben@herila.net>

Boost Software License - Version 1.0 - August 17th, 2003

Permission is hereby granted, free of charge, to any person or organization obtaining a copy of the 
software and accompanying documentation covered by this license (the "Software") to use, reproduce,
display, distribute, execute, and transmit the Software, and to prepare derivative works of the
Software, and to permit third-parties to whom the Software is furnished to do so, all subject to 
the following:

The copyright notices in the Software and this entire statement, including the above license grant,
this restriction and the following disclaimer, must be included in all copies of the Software, in
whole or in part, and all derivative works of the Software, unless such copies or derivative works 
are solely in the form of machine-executable object code generated by a source language processor.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT 
NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE, TITLE AND NON-
INFRINGEMENT. IN NO EVENT SHALL THE COPYRIGHT HOLDERS OR ANYONE DISTRIBUTING THE SOFTWARE BE LIABLE
FOR ANY DAMAGES OR OTHER LIABILITY, WHETHER IN CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR
IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

*************************************************************************************************/


using System;
using System.Diagnostics;
using N2.Collections;
using N2.Details;
using N2.Integrity;
using System.Collections.Generic;

namespace N2.Web
{
	public enum HeadingLevel
	{
		H1 = 1,
		H2 = 2,
		H3 = 3,
		H4 = 4
	}

	public static class HeadingLevelUtility
	{
		public static string DoTitle(HeadingLevel hl, string title)
		{
			switch (hl)
			{
				case HeadingLevel.H1:
					return "<h1>" + title + "</h1>";
				case HeadingLevel.H2:
					return "<h2>" + title + "</h2>";
				case HeadingLevel.H3:
					return "<h3>" + title + "</h3>";
				case HeadingLevel.H4:
					return "<h4>" + title + "</h4>";
				default:
					throw new ArgumentException("hl");
			}
		}

	}

	public enum NewsDisplayMode
	{
		TitleLinkOnly = 0,
		TitleAndAbstract = 1,
		TitleAndText = 2,
		HtmlItemTemplate = 3
	}

	public enum SortDirection
	{
		Descending = 0,
		Ascending = 1
	}

	public enum ContentSortMode
	{
		None = -1,
		Title = 1, 
		PublishDate = 0,
		Expiration = 2
	}

	[PartDefinition("ContentContainer Link", IconClass = "n2-icon-link n2-blue")]
	[RestrictParents(typeof (ContentList))]
	public class ContentListContainerLink : ContentItem, Definitions.IPart
	{

		/// <summary>
		/// Link to a container of news or blog items.
		/// </summary>
		[EditableLink("Content Container (Parent)", 100, SelectableTypes = new[] {typeof (ContentItem)})]
		public virtual ContentItem Container
		{
			get { return (ContentItem) GetDetail("Container"); }
			set
			{
				if (!value.IsPage)
					throw new ArgumentException("value must be a page (IsPage == true)");
				SetDetail("Container", value);
			}
		}

		[EditableCheckBox("Recursive", 200, HelpText = "Include all child pages, recursively")]
		public virtual bool Recursive
		{
			get { return GetDetail("Recursive", false); }
			set { SetDetail("Recursive", value, false); }
		}
	}

	[PartDefinition("Content List",
		Description = "A list of pages that can be displayed in a column.",
		SortOrder = 160,
		IconClass = "n2-icon-list-ul n2-blue")]
	[WithEditableTitle("Title", 10, Required = false)]
	[AvailableZone("Sources", "Sources")]
	[RestrictChildren(typeof(ContentListContainerLink))]
	public class ContentList : ContentItem
	{
		public override string TemplateKey
		{
			get { return "ContentList"; }
			set { base.TemplateKey = "ContentList"; }
		}

		[EditableEnum("Title heading level", 90, typeof(HeadingLevel))]
		public virtual int TitleLevel
		{
			get { return (int)(GetDetail("TitleLevel") ?? 3); }
			set { SetDetail("TitleLevel", value, 3); }
		}

		[EditableChildren("Content Containers", "Sources", 100)]
		public virtual IList<ContentListContainerLink> Containers
		{
			get
			{
				try
				{
					var childItems = GetChildren();
					if (childItems == null)
						return new List<ContentListContainerLink>();
					return childItems.Cast<ContentListContainerLink>();
				}
				catch (Exception x)
				{
					Exceptions.Add(x.ToString());
					return new List<ContentListContainerLink>();
				}
			}
		}

		[EditableNumber("Max news to display", 120)]
		public virtual int MaxNews
		{
			get { return (int)(GetDetail("MaxNews") ?? 3); }
			set { SetDetail("MaxNews", value, 3); }
		}

		public virtual void Filter(ItemList items)
		{
			Debug.Assert(items != null, "items != null");
			PageFilter.FilterPages(items);
			CountFilter.Filter(items, 0, MaxNews);
		}

		[EditableEnum(
			Title = "Display mode",
			SortOrder = 150,
			EnumType = typeof(NewsDisplayMode))
		]
		public virtual NewsDisplayMode DisplayMode
		{
			get { return (NewsDisplayMode)(GetDetail("DisplayMode") ?? NewsDisplayMode.TitleAndAbstract); }
			set { SetDetail("DisplayMode", (int)value, (int)NewsDisplayMode.TitleAndAbstract); }
		}



		[EditableEnum(
			Title = "Sort direction",
			SortOrder = 205,
			EnumType = typeof(SortDirection))
		]
		public virtual SortDirection SortDirection
		{
			get { return (SortDirection)(GetDetail("SortDirection") ?? SortDirection.Descending); }
			set { SetDetail("SortDirection", (int)value, (int)SortDirection.Descending); }
		}

		[EditableEnum(
			Title = "Sort attribute",
			SortOrder = 200,
			EnumType = typeof(ContentSortMode))
		]
		public virtual ContentSortMode SortColumn
		{
			get { return (ContentSortMode)(GetDetail("SortColumn") ?? ContentSortMode.PublishDate); }
			set { SetDetail("SortColumn", (int)value, (int)ContentSortMode.PublishDate); }
		}

		[EditableCheckBox("Group by month", 250)]
		public virtual bool GroupByMonth
		{
			get { return (bool)(GetDetail("GroupByMonth") ?? true); }
			set { SetDetail("GroupByMonth", value, true); }
		}

		[EditableCheckBox("Show Past Items", 500, CheckBoxText = "Show Past Items")]
		public virtual bool ShowPastEvents
		{
			get { return (bool)(GetDetail("ShowPastEvents") ?? true); }
			set { SetDetail("ShowPastEvents", value, true); }
		}

		[EditableCheckBox("Show Future Items", 501, CheckBoxText = "Show Future Items")]
		public virtual bool ShowFutureEvents
		{
			get { return (bool)(GetDetail("ShowFutureEvents") ?? false); }
			set { SetDetail("ShowFutureEvents", value, false); }
		}

		[EditableCheckBox("Permissions", 502, CheckBoxText = "Evaluate and Enforce Permissions")]
		public virtual bool EnforcePermissions
		{
			get { return (bool)(GetDetail("EnforcePermissions") ?? true); }
			set { SetDetail("EnforcePermissions", value, true); }
		}

		//TODO: Make the following properties visible only if the NewsDisplayMode is set to HtmlItemTemplate
		[EditableText(
			Rows = 10, 
			Columns = 50, 
			TextMode = System.Web.UI.WebControls.TextBoxMode.MultiLine, 
			Title = "Custom Item Template (HTML)", 
			HelpText = "Enclose properties of the news/blog items in $$, e.g. $$Text$$ to insert the \"Text\" property at that location.", 
			SortOrder = 550)
		]
		public string HtmlItemTemplate
		{
			get { return GetDetail("HtmlItemTemplate", ""); }
			set { SetDetail("HtmlItemTemplate", value, ""); }
		}

		[EditableText(
			Rows = 3,
			Columns = 50,
			TextMode = System.Web.UI.WebControls.TextBoxMode.MultiLine,
			Title = "Custom Header (HTML)",
			SortOrder = 560)
		]
		public string HtmlHeader
		{
			get { return GetDetail("HtmlHeader", ""); }
			set { SetDetail("HtmlHeader", value, ""); }
		}

		[EditableText(
			Rows = 3,
			Columns = 50,
			TextMode = System.Web.UI.WebControls.TextBoxMode.MultiLine,
			Title = "Custom Footer (HTML)",
			SortOrder = 570)
		]
		public string HtmlFooter
		{
			get { return GetDetail("HtmlFooter", ""); }
			set { SetDetail("HtmlFooter", value, ""); }
		}

		public List<string> Exceptions = new List<string>();


		
	}
}