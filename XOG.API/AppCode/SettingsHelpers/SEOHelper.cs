﻿using XOG.Helpers;
using XOG.Models;
using XOG.Util;
using System;
using System.Web.UI;
using System.Xml;

namespace XOG.SettingsHelpers
{
    public static class SEOHelper
    {
        const string pageXPath = "pages/page";
        const string uniqueKey = "_PageSEO_";

        static readonly string fileName = AppConfig.SEOFile;

        public static void AddPage(string id)
        {
            if (!PageExists(id))
            {
                var xmlDoc = new XmlDocument();

                xmlDoc.Load(fileName);

                var page = xmlDoc.CreateElement("page");
                page.SetAttribute("id", id);
                xmlDoc.SelectSingleNode(pageXPath).ParentNode.AppendChild(page);

                SaveXml(xmlDoc);
            }
        }

        public static void AddPageSEO(SEO entity)
        {
            var xmlDoc = new XmlDocument();

            xmlDoc.Load(fileName);

            var newTitleMeta = xmlDoc.CreateElement("meta");
            newTitleMeta.SetAttribute("name", "title");
            newTitleMeta.SetAttribute("content", entity.Title);

            var newDescriptionMeta = xmlDoc.CreateElement("meta");
            newDescriptionMeta.SetAttribute("name", "description");
            newDescriptionMeta.SetAttribute("content", entity.Description);

            var newKeywordMeta = xmlDoc.CreateElement("meta");
            newKeywordMeta.SetAttribute("name", "keywords");
            newKeywordMeta.SetAttribute("content", entity.Keywords);

            foreach (XmlNode page in xmlDoc.SelectNodes(pageXPath))
            {
                if (page.Attributes["id"].Value == entity.ID)
                {
                    page.AppendChild(newTitleMeta);
                    page.AppendChild(newDescriptionMeta);
                    page.AppendChild(newKeywordMeta);

                    SaveXml(xmlDoc);

                    break;
                }
            }
        }

        public static void DeletePage(string id)
        {
            if (PageExists(id))
            {
                var xmlDoc = new XmlDocument();

                xmlDoc.Load(fileName);

                foreach (XmlNode page in xmlDoc.SelectNodes(pageXPath))
                {
                    if (page.Attributes["id"].Value == id)
                    {
                        page.ParentNode.RemoveChild(page);

                        SaveXml(xmlDoc);

                        break;
                    }
                }
            }
        }

        public static SEO GetPageSEO(string id)
        {
            return GetPageSEO(id, AppConfig.PerformanceMode);
        }

        public static SEO GetPageSEO(string id, PerformanceMode performanceMode)
        {
            var keyValue = new SEO();
            var performanceKey = $"{uniqueKey}_{id}";

            Func<string, SEO> fnc = GetPageSEOFromSettings;

            var args = new object[] { id };

            PerformanceHelper.GetPerformance<SEO>(performanceMode, performanceKey, out keyValue, fnc, args);

            return keyValue;
        }

        public static SEO GetPageSEOFromSettings(string id)
        {
            var entity = new SEO()
            {
                ID = id
            };
            var xmlDoc = new XmlDocument();

            xmlDoc.Load(fileName);

            foreach (XmlNode page in xmlDoc.SelectNodes(pageXPath))
            {
                if (page.Attributes["id"].Value == id)
                {
                    foreach (XmlNode meta in page.ChildNodes)
                    {
                        if ("keywords" == meta.Attributes["name"].Value)
                        {
                            entity.Keywords = meta.Attributes["content"].Value;
                        }

                        if ("title" == meta.Attributes["name"].Value)
                        {
                            entity.Title = meta.Attributes["content"].Value;
                        }

                        if ("description" == meta.Attributes["name"].Value)
                        {
                            entity.Description = meta.Attributes["content"].Value;
                        }
                    }

                    break;
                }
            }

            return entity;
        }

        public static bool PageExists(string id)
        {
            var present = false;

            var xmlDoc = new XmlDocument();

            xmlDoc.Load(fileName);

            foreach (XmlNode page in xmlDoc.SelectNodes(pageXPath))
            {
                if (id == page.Attributes["id"].Value)
                {
                    present = true;
                    break;
                }
            }

            return present;
        }

        public static void SetPageSEO(Page page, SEO seo)
        {
            var title = seo.Title;

            if (title.NullableContains("{ONLY-SITENAME}"))
            {
                title = title.Replace("{ONLY-SITENAME}", string.Empty);

                page.Title = $"{AppConfig.SiteName}";
            }

            if (title.NullableContains("{ONLY-TITLE}"))
            {
                title = title.Replace("{ONLY-TITLE}", string.Empty);

                page.Title = $"{title}";
            }

            if (!seo.Title.NullableContains("{ONLY-SITENAME}") && !seo.Title.NullableContains("{ONLY-TITLE}"))
            {
                page.Title = $"{seo.Title} | {AppConfig.SiteName}";
            }

            page.MetaDescription = seo.Description;
            page.MetaKeywords = seo.Keywords;
        }

        public static void SetPageSEO(SEO entity)
        {
            if (PageExists(entity.ID))
            {
                var xmlDoc = new XmlDocument();

                xmlDoc.Load(fileName);

                foreach (XmlNode page in xmlDoc.SelectNodes(pageXPath))
                {
                    if (page.Attributes["id"].Value == entity.ID)
                    {
                        foreach (XmlNode meta in page.ChildNodes)
                        {
                            if ("keywords" == meta.Attributes["name"].Value)
                            {
                                meta.Attributes["content"].Value = entity.Keywords;
                            }

                            if ("title" == meta.Attributes["name"].Value)
                            {
                                meta.Attributes["content"].Value = entity.Title;
                            }

                            if ("description" == meta.Attributes["name"].Value)
                            {
                                meta.Attributes["content"].Value = entity.Description;
                            }
                        }

                        SaveXml(xmlDoc);

                        break;
                    }
                }
            }
        }

        static void SaveXml(XmlDocument xmlDoc)
        {
            var xmlTextWriter = new XmlTextWriter(fileName, null)
            {
                Formatting = Formatting.Indented
            };
            xmlDoc.WriteContentTo(xmlTextWriter);
            xmlTextWriter.Close();
        }
    }
}
