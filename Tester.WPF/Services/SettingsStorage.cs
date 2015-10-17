using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using Tester.Properties;

namespace Tester.Services
{
	public class SettingsStorage
	{
        public static readonly SettingsStorage Instance = new SettingsStorage();

        private const string SettingsFileName = "Settings.txt";

        private const string PathPropertyName = "Path";
        private const string LeftPropertyName = "Left";
        private const string TopPropertyName = "Top";
        private const string WidthPropertyName = "Width";
        private const string HeightPropertyName = "Height";
        private const string IsTopmostPropertyName = "IsTopMost";

        private string GetSettingsFullPath()
        {
            var applicationDirectory = System.AppDomain.CurrentDomain.BaseDirectory;
            var settingsPath = Path.Combine(applicationDirectory, SettingsFileName);
            return settingsPath;
        }

        private readonly Dictionary<string, SettingsProperty> _properties = new Dictionary<string, SettingsProperty>();

		private readonly ViewModelProperty<string> _pathProperty = new ViewModelProperty<string>();
		private readonly ViewModelProperty<int> _leftProperty = new ViewModelProperty<int>();
		private readonly ViewModelProperty<int> _topProperty = new ViewModelProperty<int>();
		private readonly ViewModelProperty<int> _widthProperty = new ViewModelProperty<int>();
		private readonly ViewModelProperty<int> _heightProperty = new ViewModelProperty<int>();
		private readonly ViewModelProperty<bool> _isTopmostProprety = new ViewModelProperty<bool>();

		public ViewModelProperty<string> PathProperty
		{
			get { return _pathProperty; }
		}

        public ViewModelProperty<int> LeftProperty
		{
			get { return _leftProperty; }
		}

		public ViewModelProperty<int> TopProperty
		{
			get { return _topProperty; }
		}

		public ViewModelProperty<int> WidthProperty
		{
			get { return _widthProperty; }
		}

		public ViewModelProperty<int> HeightProperty
		{
			get { return _heightProperty; }
		}

		public ViewModelProperty<bool> IsTopmostProperty
		{
			get { return _isTopmostProprety; }
		}

		private SettingsStorage()
		{
            ConfigureProperties();

			LoadSettings();

			_pathProperty.OnChanged(SaveSettings);
			_leftProperty.OnChanged(SaveSettings);
			_topProperty.OnChanged(SaveSettings);
			_widthProperty.OnChanged(SaveSettings);
			_heightProperty.OnChanged(SaveSettings);
			_isTopmostProprety.OnChanged(SaveSettings);
		}

        public void OpenSettingsInExternalEditor()
        {
            var path = GetSettingsFullPath();
            Process.Start(path);
        }

        private void ConfigureProperties()
        {
            _properties.Add(PathPropertyName, ConfigureStringProperty(PathProperty));
            _properties.Add(LeftPropertyName, ConfigureIntProperty(LeftProperty));
            _properties.Add(TopPropertyName, ConfigureIntProperty(TopProperty));
            _properties.Add(WidthPropertyName, ConfigureIntProperty(WidthProperty));
            _properties.Add(HeightPropertyName, ConfigureIntProperty(HeightProperty));
            _properties.Add(IsTopmostPropertyName, ConfigureBoolProperty(IsTopmostProperty));
        }

        private SettingsProperty ConfigureStringProperty(ViewModelProperty<string> property)
        {
            return new SettingsProperty(s => property.Value = s, () => property.Value);
        }

        private SettingsProperty ConfigureIntProperty(ViewModelProperty<int> property)
        {
            return new SettingsProperty(s =>
            {
                int value;
                if (int.TryParse(s, NumberStyles.Number, CultureInfo.InvariantCulture, out value)) property.Value = value;
            }, () => property.Value.ToString(CultureInfo.InvariantCulture));
        }

        private SettingsProperty ConfigureBoolProperty(ViewModelProperty<bool> property)
        {
            return new SettingsProperty(s =>
            {
                bool value;
                if (bool.TryParse(s, out value)) property.Value = value;
            }, () => property.Value.ToString(CultureInfo.InvariantCulture));
        }

        private Dictionary<string, string> ReadSettings()
        {
            var settingsPath = GetSettingsFullPath();
            if (!File.Exists(settingsPath)) return null;

            var settingsContent = File.ReadAllLines(settingsPath);
            return settingsContent
                        .Select(l => l.Split(new[] { ':' }, 2))
                        .ToDictionary(ss => ss[0], ss => ss[1]);
        }

		private void LoadSettings()
		{
            var settings = ReadSettings();
            if (settings == null) return;

            foreach (var kvp in settings)
            {
                var key = kvp.Key.Trim();
                var value = kvp.Value.Trim();
                SettingsProperty property;
                if (!_properties.TryGetValue(key, out property)) continue;
                property.TryParse(value);
            }
		}

		private void SaveSettings()
		{
            var settings = _properties
                                .Select(kvp => kvp.Key + " : " + kvp.Value.GetValue())
                                .ToArray();

            File.WriteAllLines(GetSettingsFullPath(), settings);
		}

        class SettingsProperty
        {
            private readonly Action<string> _parser;
            private readonly Func<string> _valueGetter;

            public SettingsProperty(Action<string> parser, Func<string> valueGetter)
            {
                _parser = parser;
                _valueGetter = valueGetter;
            }

            public void TryParse(string s)
            {
                _parser.Invoke(s);
            }

            public string GetValue()
            {
                return _valueGetter.Invoke();
            }
        }
	}
}