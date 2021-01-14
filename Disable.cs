using System;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.ArcMapUI;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Framework;
using ESRI.ArcGIS.ADF.CATIDs;

namespace DisableExtensions
{
    [Guid("c48ff891-c261-481c-9eee-96dbddaf954b")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("DisableExtensions.Disable")]
    public class Disable : IExtension //, IPersistVariant
    {
        #region COM Registration Function(s)
        [ComRegisterFunction()]
        [ComVisible(false)]

        static void RegisterFunction(Type registerType)
        {
            // Required for ArcGIS Component Category Registrar support
            ArcGISCategoryRegistration(registerType);

            //
            // TODO: Add any COM registration code here
            //
        }

        [ComUnregisterFunction()]
        [ComVisible(false)]
        static void UnregisterFunction(Type registerType)
        {
            // Required for ArcGIS Component Category Registrar support
            ArcGISCategoryUnregistration(registerType);

            //
            // TODO: Add any COM unregistration code here
            //
        }

        #region ArcGIS Component Category Registrar generated code
        /// <summary>
        /// Required method for ArcGIS Component Category registration -
        /// Do not modify the contents of this method with the code editor.
        /// </summary>
        private static void ArcGISCategoryRegistration(Type registerType)
        {
            string regKey = string.Format("HKEY_CLASSES_ROOT\\CLSID\\{{{0}}}", registerType.GUID);
            MxExtension.Register(regKey);
            //GMxExtensions.Register(regKey);
            //SxExtensions.Register(regKey);
        }
        /// <summary>
        /// Required method for ArcGIS Component Category unregistration -
        /// Do not modify the contents of this method with the code editor.
        /// </summary>
        private static void ArcGISCategoryUnregistration(Type registerType)
        {
            string regKey = string.Format("HKEY_CLASSES_ROOT\\CLSID\\{{{0}}}", registerType.GUID);
            MxExtension.Unregister(regKey);
            //GMxExtensions.Unregister(regKey);
            //SxExtensions.Unregister(regKey);
        }

        #endregion
        #endregion

        //Event member variable.
        private IDocumentEvents_Event m_docEvents = null;
        private IApplication m_application;

        //Wiring.
        private void SetUpDocumentEvent(IDocument myDocument)
        {
            m_docEvents = myDocument as IDocumentEvents_Event;
            m_docEvents.CloseDocument += new IDocumentEvents_CloseDocumentEventHandler(Disable_CloseDocument);
        }

        //Event handler method.
        void Disable_CloseDocument()
        {
            IExtensionManager extManager = m_application as IExtensionManager;
            findAndDisableLoadedExtensions(extManager);
            findAndDisableNonLoadedJITExtensions(extManager);
        }

        private void findAndDisableLoadedExtensions(IExtensionManager extManager)
        {
            for (int i = 0; i < extManager.ExtensionCount; i++)
            {
                IExtension ext = extManager.get_Extension(i);
                unloadExtension(ext);
            }
        }

        private void findAndDisableNonLoadedJITExtensions(IExtensionManager extManager)
        {
            IJITExtensionManager jitExtManager = m_application as IJITExtensionManager;

            for (int i = 0; i < jitExtManager.JITExtensionCount; i++)
            {
                UID extID = jitExtManager.get_JITExtensionCLSID(i);
                IExtension ext = m_application.FindExtensionByCLSID(extID);
                unloadExtension(ext);
            }
        }

        private void unloadExtension(IExtension ext)
        {
            if (ext != null)
            {
                if (ext is IExtensionConfig)
                {
                    IExtensionConfig extConfig = (IExtensionConfig)ext;
                    if (extConfig.State == esriExtensionState.esriESEnabled) extConfig.State = esriExtensionState.esriESDisabled;
                }
            }
        }

        #region "IExtension Implementations"
        public string Name
        {
            get
            {
                return "DisableExtensions";
            }
        }

        public IDocumentEvents_Event DocEvents { get => m_docEvents; set => m_docEvents = value; }

        public void Shutdown()
        {            
            DocEvents = null;
            m_application = null;
        }

        public void Startup(ref object initializationData)
        {
            m_application = initializationData as IApplication;
			 if (m_application == null)
                return;
            SetUpDocumentEvent(m_application.Document);
        }
        #endregion

    }
}