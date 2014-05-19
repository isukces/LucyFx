﻿using Nancy.ViewEngines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nancy.Extensions;
using Nancy.ViewEngines.Razor;

namespace Lucy.Bundle
{
    public sealed class HtmlRenderer
    {
        #region Constructors

        internal HtmlRenderer(IRenderContext renderContext, BundleTypes supportedBundleType)
        {
            _renderContext = renderContext;
            _supportedBundleType = supportedBundleType;
        }

        #endregion Constructors

        #region Methods

        // Public Methods 

        public IHtmlString Render(string bundlePath)
        {
            var bundle = RegisteredBundles.GetBundleByName(bundlePath);
            if (bundle == null)
                throw new Exception(string.Format("Try to render unregistered bundle {0}", bundlePath));
            return Render(bundle);
        }

        internal IHtmlString Render(Bundle bundle)
        {
            if (bundle.BundleType != _supportedBundleType)
                throw new Exception(string.Format("Bundle {0} type is {1}, expected {2}.", bundle.VirtualPath, bundle.BundleType,
                    _supportedBundleType));
            LucyToys lucyToys = LucyToys.GetOrCreate(_renderContext.Context.ViewBag);

            var filesWithDependencies = bundle.FilesWithDependencies;
            var originalFilesCount = filesWithDependencies.Count;
            if (lucyToys.RenderedFiles.Count > 0)
                filesWithDependencies = (from file in filesWithDependencies
                    where lucyToys.RenderedFiles.IndexOf(file) < 0
                    select file).ToList();
            IEnumerable<string> fullPaths;
            if (BundleSettings.processing == BundleProcessing.ManyFiles)
                fullPaths = from file in filesWithDependencies
                    select file.GetFullUriPath(_renderContext.Context);
            else
            {
                var path = filesWithDependencies.Count == originalFilesCount
                    ? bundle.VirtualPath
                    : BundleSettings.MakePath(Bundle.DynamicBundleNameFromFiles(filesWithDependencies), _supportedBundleType);
                fullPaths = new[]
                {
                    _renderContext.Context.ToFullPath(path)
                };
            }


            lucyToys.RenderedFiles.AddRange(filesWithDependencies);
            var template = BundleSettings.GetRenderTemplate(_supportedBundleType);
            var separator = BundleSettings.GetSeparator(_supportedBundleType);
            var stringBuilder = new StringBuilder();
            foreach (var fullPath in fullPaths)
            {
                if (stringBuilder.Length > 0)
                    stringBuilder.Append(separator);
                stringBuilder.AppendFormat(template, fullPath);
            }
            return new NonEncodedHtmlString(stringBuilder.ToString());
        }

        #endregion Methods

        #region Fields

        private readonly IRenderContext _renderContext;
        private readonly BundleTypes _supportedBundleType;

        #endregion Fields
    }
}
