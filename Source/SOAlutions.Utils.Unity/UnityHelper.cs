// © 2013 SOAlutions, Inc. All rights reserved.
// Please direct all inquiries to http://www.soalutions.net

using System;
using System.Configuration;
using System.IO;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using SOAlutions.Utils;

public class UnityHelper
{
    private const string UnityConfigurationSectionName = "unity";
    private const string UnityContainerNameKey = "unityContainerName";

    public static IUnityContainer ConfigureUnityContainer()
    {
        // Get the unity configuration section from the default config file hierarchy
        UnityConfigurationSection section = ConfigurationManager.GetSection(UnityConfigurationSectionName) as UnityConfigurationSection;

        return ConfigureUnityConfigurationSection(section);
    }

    private static IUnityContainer ConfigureUnityConfigurationSection(UnityConfigurationSection section)
    {
        string unityContainerName = ConfigurationHelper.GetAppSetting(UnityContainerNameKey);

        // Create and populate a new UnityContainer with the configuration information.
        IUnityContainer container = new UnityContainer();

        if (String.IsNullOrEmpty(unityContainerName))
        {
            container.LoadConfiguration(section);
        }
        else
        {
            container.LoadConfiguration(section, unityContainerName);
        }

        // Create service locator (optional)
        ServiceLocator.SetLocatorProvider(() => new UnityServiceLocator(container));

        return container;
    }

    public static IUnityContainer ConfigureUnityContainer(string configFileName)
    {
        string configFileDirectoryPath = AppDomain.CurrentDomain.BaseDirectory;

        return ConfigureUnityContainer(configFileName, configFileDirectoryPath);
    }

    public static IUnityContainer ConfigureUnityContainer(string configFileName, string configFileDirectoryPath)
    {
        string configFilePath = Path.Combine(configFileDirectoryPath, configFileName);

        ExeConfigurationFileMap map = new ExeConfigurationFileMap();
        map.ExeConfigFilename = configFilePath;
        Configuration config = ConfigurationManager.OpenMappedExeConfiguration(map, ConfigurationUserLevel.None);
        UnityConfigurationSection section = config.GetSection(UnityConfigurationSectionName) as UnityConfigurationSection;

        return ConfigureUnityConfigurationSection(section);
    }
}