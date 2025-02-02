﻿using System;
using System.Web;

namespace XOG.Models
{
    public class TagCloudItem
    {
        string _hovertitle;
        int _scaleValue;
        string _text;

        public TagCloudItem(string text, string url, int count)
        {
            this._text = text;
            this.Url = url;
            this.Count = (Decimal)count;
        }

        public TagCloudItem(string text, string hoverTitle, string url, int count)
        {
            this._text = text;
            this.Url = url;
            this.Count = (Decimal)count;
            this._hovertitle = hoverTitle;
        }

        public Decimal Count
        {
            get;
            set;
        }

        public string HoverTitle
        {
            get
            {
                return HttpContext.Current.Server.HtmlEncode(this._hovertitle);
            }

            set
            {
                this._hovertitle = value;
            }
        }

        public int ScaleValue
        {
            get
            {
                return this._scaleValue;
            }
        }

        public string Text
        {
            get
            {
                return HttpContext.Current.Server.HtmlEncode(this._text);
            }

            set
            {
                this._text = value;
            }
        }

        public string Url
        {
            get;
            set;
        }

        public void SetScaleValue(int scaleValue)
        {
            this._scaleValue = scaleValue;
        }
    }
}
