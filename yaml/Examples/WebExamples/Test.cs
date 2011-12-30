using System;
using Yaml;

namespace WebExamples
{
	/// <summary>
	/// The examples that we have used on http://lumumba.uhasselt.be/~christophe/YAML/#example
	/// </summary>
	public class Test
	{
		static void Main(string[] args)
		{
			Console.WriteLine("Reading from a file");
			Console.WriteLine("--------------------------");
			Node node = Node.FromFile ("testRead.yaml");
			Console.WriteLine (node);

			Console.WriteLine("Reading from a string");
			Console.WriteLine("--------------------------");
			node = Node.Parse ("- item1\n- item2\n");
			Console.WriteLine (node);

			Console.WriteLine("Writing to a file");
			Console.WriteLine("--------------------------");
			node.ToFile ("testWrite.yaml");

			Console.WriteLine("Writing to a string");
			Console.WriteLine("--------------------------");
			string s = node.Write ();
			Console.WriteLine( s );

			Console.WriteLine("Creating a YAML tree");
			Console.WriteLine("--------------------------");
			Sequence sequence = new Sequence (
							new Node []
							{
								new Yaml.String ("item 1"),
								new Yaml.String ("item 2"),
								new Yaml.String ("item 3"),

								new Mapping (
									new MappingNode []
									{
										new MappingNode (new Yaml.String ("key 2"), new Yaml.String ("value 1")),
										new MappingNode (new Yaml.String ("key 2"), new Yaml.String ("value 2"))
									} ),

									new Yaml.String ("item 5")
							} );

			Console.WriteLine (sequence);

			Console.WriteLine("Traversing a YAML tree");
			Console.WriteLine("--------------------------");
			// Iets dat nog moet gebeuren
			foreach (Node n in sequence.Nodes)
			{
				if (n.Type == NodeType.String)
					Console.Write ("Found a string: " + ((Yaml.String) n).Content + "\n");
			}
		}
	}
}
