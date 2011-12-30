// oud bestand, mag verwijderd worden


namespace Yaml
{
	class YamlNode
	{
		public static YamlNode Parse (string yaml)
		{
			...
			... new YamlNode ();
			...
		}
		public static YamlNode ParseFromFile (string filename);

		public void WriteToFile (string filename);
		public string Write ();
	}

	class YamlMapping : YamlNode
	{
		Hash data;

		AddNode (YamlNode key, YamlNode value);
	}

	class YamlSequence : YamlNode
	{
		ArrayList data;

		AddNode (YamlNode);
	}

	class YamlScalar : YamlNode
	{
		public static YamlScalar ParseScalar (string scalar);
		public string WriteScalar ();

		public abstract string Type ();
	}

		class YamlDate : YamlScalar
		{
		}
		class YamlString : YamlScalar
		{
		}
		class YamlNumber :
}

