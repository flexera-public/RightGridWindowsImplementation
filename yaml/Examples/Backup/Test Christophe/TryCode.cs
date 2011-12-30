using System;
using Yaml;

namespace Test_Christophe
{
	/// <summary>
	/// Summary description for TryCode.
	/// </summary>
	public class TryCode
	{
		public TryCode() { }

		public static void base64()
		{
			/*string bin = "AAECAwQFBgcICQoLDA0ODxAREhMUFRYXGBkaGxwdHh8gISIjJCUmJygpKissLS4vMDEyMzQ1Njc4" + 
					"OTo7PD0+P0BBQkNERUZHSElKS0xNTk9QUVJTVFVWV1hZWltcXV5fYGFiY2RlZmdoaWprbG1ub3Bx" + 
					"cnN0dXZ3eHl6e3x9fn+AgYKDhIWGh4iJiouMjY6PkJGSk5SVlpeYmZqbnJ2en6ChoqOkpaanqKmq" +
					"q6ytrq+wsbKztLW2t7i5uru8vb6/wMHCw8TFxsfIycrLzM3Oz9DR0tPU1dbX2Nna29zd3t/g4eLj" +
					"5OXm5+jp6uvs7e7v8PHy8/T19vf4+fr7/P3+/w==";*/
			string bin = "R0lGODlhDAAMAIQAAP//9/X17unp5WZmZgAAAOfn515eXvPz7Y6OjuDg4J+fn5" +
				"OTk6enp56enmlpaWNjY6Ojo4SEhP/++f/++f/++f/++f/++f/++f/++f/++f/+" +
				"f/++f/++f/++f/++f/++SH+Dk1hZGUgd2l0aCBHSU1QACwAAAAADAAMAAAFLC" +
				"AgjoEwnuNAFOhpEMTRiggcz4BNJHrv/zCFcLiwMWYNG84BwwEeECcgggoBADs";
			char[] array = bin.ToCharArray();
				
			byte[] hulp;
			hulp = System.Convert.FromBase64CharArray(array, 0, array.Length);

			for(int i = 0; i < hulp.Length; i++)
				Console.Write(hulp[i] + " ");
		}

		public static void binary()
		{
			string bin = "Dit is de typische Hello World zin.";
			byte[] array = System.Text.Encoding.ASCII.GetBytes(bin);

			Binary test = new Binary(array);

			Console.WriteLine(test.ToString());
		}

		public static void int32()
		{
			System.Int32 a = new System.Int32 ();
			a = 15;
			telop (a);
			Console.WriteLine (a);
		}

		public static void boxing()
		{
			int test = 20;
			object ref1 = (object) test;
			object ref2 = ref1;

			Console.WriteLine("ref1 = " + ref1);
			Console.WriteLine("ref2 = " + ref2);

			ref1 = ((int) test) + 20;
				
			Console.WriteLine("ref1 = " + ref1);
			Console.WriteLine("ref2 = " + ref2);
		}

		// Zie:
		// http://msdn2.microsoft.com/en-US/library/0f66670z(VS.80).aspx
		public static void refer()
		{
			// We maken een integer object met waarde 15
			System.Int32 a = new System.Int32 ();
			a = 15;
			Console.WriteLine (a);

			// Geven dit als parameter mee zonder cast
			telop (a);
			Console.WriteLine (a);
			
			// Schrijft nog steeds 15 uit ->

			// Wordt dit toch by value doorgegeven? of wordt er telkens
			// er iets gewijzigd wordt aan System.Int32 een nieuwe instantie
			// aangemaakt zoals ook bij 'string' het geval is?

			// Boxing naar object
			telop2 ((object) a);
			Console.WriteLine (a);

			// Via 'ref'
			telop3 (ref a);
			Console.WriteLine (a);
			// Schrijft wel 16 uit
		}
		static private void telop (System.Int32 a)
		{
			a ++;
		}
		static private void telop2 (Object a)
		{
			System.Int32 b = ((int) a);
			b ++;
		}
		static private void telop3 (ref System.Int32 a)
		{
			a ++;
		}
	}
}
