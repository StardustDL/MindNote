﻿using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using MindNote.Client.API;

namespace MindNote.Server.Host.TagHelpers
{
    [HtmlTargetElement("tag-display")]
    public class TagDisplayTagHelper : TagHelper
    {
        public Tag Value { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (Value != null)
            {
                output.TagName = "span";
                output.TagMode = TagMode.StartTagAndEndTag;

                TagBuilder color = new TagBuilder("i");
                color.AddCssClass("fa");
                color.AddCssClass("fa-tag");
                // "display:inline-block; border-radius:50%; height:12px; width:12px; position:relative;"
                color.Attributes["style"] = (Value.Color != null ? $"color:{Value.Color}" : "");
                output.Content.AppendHtml(color);

                TagBuilder name = new TagBuilder("span");
                name.Attributes["style"] = "margin-left: 10px";
                name.InnerHtml.Append(Value.Name);
                output.Content.AppendHtml(name);
            }
            base.Process(context, output);
        }
    }
}
