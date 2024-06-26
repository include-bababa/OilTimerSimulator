using System.IO;
using System.Text;
using System.Xml;

using UnityEditor;
using UnityEditor.Android;

public class ModifyUnityAndroidAppManifestSample : IPostGenerateGradleAndroidProject
{
    private string manifestFilePath;

    public void OnPostGenerateGradleAndroidProject(string basePath)
    {
        // If needed, add condition checks on whether you need to run the modification routine.
        // For example, specific configuration/app options enabled

        var androidManifest = new AndroidManifest(this.GetManifestPath(basePath));

        // Add your XML manipulation routines
#if FIREBASE_USE_TEST_LOOP
        androidManifest.SetTestLoop();
#endif

        androidManifest.Save();
    }

    public int callbackOrder { get { return 1; } }

    private string GetManifestPath(string basePath)
    {
        if (string.IsNullOrEmpty(this.manifestFilePath))
        {
            var pathBuilder = new StringBuilder(basePath);
            pathBuilder.Append(Path.DirectorySeparatorChar).Append("src");
            pathBuilder.Append(Path.DirectorySeparatorChar).Append("main");
            pathBuilder.Append(Path.DirectorySeparatorChar).Append("AndroidManifest.xml");
            this.manifestFilePath = pathBuilder.ToString();
        }
        return this.manifestFilePath;
    }
}


internal class AndroidXmlDocument : XmlDocument
{
    public readonly string AndroidXmlNamespace = "http://schemas.android.com/apk/res/android";

    private string filepath;
    private XmlNamespaceManager namespaceManager;

    public AndroidXmlDocument(string path)
    {
        this.filepath = path;
        using (var reader = new XmlTextReader(this.filepath))
        {
            reader.Read();
            this.Load(reader);
        }

        this.namespaceManager = new XmlNamespaceManager(this.NameTable);
        this.namespaceManager.AddNamespace("android", AndroidXmlNamespace);
    }

    public string FilePath => this.filepath;

    public XmlNamespaceManager AndroidNamespaceManager => this.namespaceManager;

    public string Save()
    {
        return this.SaveAs(this.filepath);
    }

    public string SaveAs(string path)
    {
        using (var writer = new XmlTextWriter(path, new UTF8Encoding(false)))
        {
            writer.Formatting = Formatting.Indented;
            this.Save(writer);
        }
        return path;
    }
}


internal class AndroidManifest : AndroidXmlDocument
{
    private readonly XmlElement applicationElement;

    public AndroidManifest(string path)
        : base(path)
    {
        this.applicationElement = this.SelectSingleNode("/manifest/application") as XmlElement;
    }

    private XmlAttribute CreateAndroidAttribute(string key, string value)
    {
        XmlAttribute attr = this.CreateAttribute("android", key, AndroidXmlNamespace);
        attr.Value = value;
        return attr;
    }

    internal XmlNode GetActivityWithLaunchIntent()
    {
        return this.SelectSingleNode("/manifest/application/activity[intent-filter/action/@android:name='android.intent.action.MAIN' and " +
                "intent-filter/category/@android:name='android.intent.category.LAUNCHER']", this.AndroidNamespaceManager);
    }

    internal void SetApplicationTheme(string appTheme)
    {
        this.applicationElement.Attributes.Append(this.CreateAndroidAttribute("theme", appTheme));
    }

    internal void SetStartingActivityName(string activityName)
    {
        this.GetActivityWithLaunchIntent().Attributes.Append(this.CreateAndroidAttribute("name", activityName));
    }


    internal void SetHardwareAcceleration()
    {
        this.GetActivityWithLaunchIntent().Attributes.Append(this.CreateAndroidAttribute("hardwareAccelerated", "true"));
    }

    internal void SetMicrophonePermission()
    {
        var manifest = this.SelectSingleNode("/manifest");
        XmlElement child = this.CreateElement("uses-permission");
        manifest.AppendChild(child);
        XmlAttribute newAttribute = this.CreateAndroidAttribute("name", "android.permission.RECORD_AUDIO");
        child.Attributes.Append(newAttribute);
    }

    internal void SetTestLoop()
    {
        var intentFilter = this.CreateElement("intent-filter");
        {
            XmlElement child = this.CreateElement("action");
            intentFilter.AppendChild(child);
            XmlAttribute newAttribute = this.CreateAndroidAttribute("name", "com.google.intent.action.TEST_LOOP");
            child.Attributes.Append(newAttribute);
        }
        {
            XmlElement child = this.CreateElement("category");
            intentFilter.AppendChild(child);
            XmlAttribute newAttribute = this.CreateAndroidAttribute("name", "android.intent.category.DEFAULT");
            child.Attributes.Append(newAttribute);
        }
        {
            XmlElement child = this.CreateElement("data");
            intentFilter.AppendChild(child);
            XmlAttribute newAttribute = this.CreateAndroidAttribute("mimeType", "application/javascript");
            child.Attributes.Append(newAttribute);
        }

        this.GetActivityWithLaunchIntent().AppendChild(intentFilter);

        var application = this.SelectSingleNode("/manifest/application");
        {
            var child = this.CreateElement("meta-data");
            application.AppendChild(child);
            child.Attributes.Append(this.CreateAndroidAttribute("name", "com.google.test.loops"));
            child.Attributes.Append(this.CreateAndroidAttribute("value", "2"));
        }
        {
            var child = this.CreateElement("meta-data");
            application.AppendChild(child);
            child.Attributes.Append(this.CreateAndroidAttribute("name", "com.google.test.loops.SIMPLE_TEST"));
            child.Attributes.Append(this.CreateAndroidAttribute("value", "1,2"));
        }
    }
}