﻿//
// HeaderListTests.cs
//
// Author: Jeffrey Stedfast <jeff@xamarin.com>
//
// Copyright (c) 2013-2016 Xamarin Inc. (www.xamarin.com)
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//

using System;
using System.IO;
using System.Text;

using NUnit.Framework;

using MimeKit;

namespace UnitTests {
	[TestFixture]
	public class HeaderListTests
	{
		[Test]
		public void TestArgumentExceptions ()
		{
			var list = new HeaderList ();
			Header header;
			string value;

			// Add
			Assert.Throws<ArgumentNullException> (() => list.Add (null));
			Assert.Throws<ArgumentOutOfRangeException> (() => list.Add (HeaderId.Unknown, "value"));
			Assert.Throws<ArgumentNullException> (() => list.Add (HeaderId.AdHoc, null));
			Assert.Throws<ArgumentNullException> (() => list.Add (null, "value"));
			Assert.Throws<ArgumentNullException> (() => list.Add ("field", null));
			Assert.Throws<ArgumentOutOfRangeException> (() => list.Add (HeaderId.Unknown, Encoding.UTF8, "value"));
			Assert.Throws<ArgumentNullException> (() => list.Add (HeaderId.AdHoc, null, "value"));
			Assert.Throws<ArgumentNullException> (() => list.Add (HeaderId.AdHoc, Encoding.UTF8, null));
			Assert.Throws<ArgumentNullException> (() => list.Add (null, Encoding.UTF8, "value"));
			Assert.Throws<ArgumentNullException> (() => list.Add ("field", null, "value"));
			Assert.Throws<ArgumentNullException> (() => list.Add ("field", Encoding.UTF8, null));

			// Contains
			Assert.Throws<ArgumentOutOfRangeException> (() => list.Contains (HeaderId.Unknown));
			Assert.Throws<ArgumentNullException> (() => list.Contains ((Header) null));
			Assert.Throws<ArgumentNullException> (() => list.Contains ((string) null));

			// CopyTo
			Assert.Throws<ArgumentOutOfRangeException> (() => list.CopyTo (new Header[0], -1));
			Assert.Throws<ArgumentNullException> (() => list.CopyTo (null, 0));

			// IndexOf
			Assert.Throws<ArgumentOutOfRangeException> (() => list.IndexOf (HeaderId.Unknown));
			Assert.Throws<ArgumentNullException> (() => list.IndexOf ((Header) null));
			Assert.Throws<ArgumentNullException> (() => list.IndexOf ((string) null));

			// Insert
			list.Add ("field", "value");
			Assert.Throws<ArgumentOutOfRangeException> (() => list.Insert (-1, new Header (HeaderId.AdHoc, "value")));
			Assert.Throws<ArgumentOutOfRangeException> (() => list.Insert (-1, HeaderId.AdHoc, Encoding.UTF8, "value"));
			Assert.Throws<ArgumentOutOfRangeException> (() => list.Insert (-1, "field", Encoding.UTF8, "value"));
			Assert.Throws<ArgumentOutOfRangeException> (() => list.Insert (-1, HeaderId.AdHoc, "value"));
			Assert.Throws<ArgumentOutOfRangeException> (() => list.Insert (-1, "field", "value"));
			Assert.Throws<ArgumentOutOfRangeException> (() => list.Insert (0, HeaderId.Unknown, Encoding.UTF8, "value"));
			Assert.Throws<ArgumentOutOfRangeException> (() => list.Insert (0, HeaderId.Unknown, "value"));
			Assert.Throws<ArgumentNullException> (() => list.Insert (0, HeaderId.AdHoc, Encoding.UTF8, null));
			Assert.Throws<ArgumentNullException> (() => list.Insert (0, HeaderId.AdHoc, null, "value"));
			Assert.Throws<ArgumentNullException> (() => list.Insert (0, HeaderId.AdHoc, null));
			Assert.Throws<ArgumentNullException> (() => list.Insert (0, null, "value"));
			Assert.Throws<ArgumentNullException> (() => list.Insert (0, "field", null));
			Assert.Throws<ArgumentNullException> (() => list.Insert (0, null));

			// LastIndexOf
			Assert.Throws<ArgumentOutOfRangeException> (() => list.LastIndexOf (HeaderId.Unknown));
			Assert.Throws<ArgumentNullException> (() => list.LastIndexOf ((string) null));

			// Remove
			Assert.Throws<ArgumentOutOfRangeException> (() => list.Remove (HeaderId.Unknown));
			Assert.Throws<ArgumentNullException> (() => list.Remove ((Header) null));
			Assert.Throws<ArgumentNullException> (() => list.Remove ((string) null));

			// RemoveAll
			Assert.Throws<ArgumentOutOfRangeException> (() => list.RemoveAll (HeaderId.Unknown));
			Assert.Throws<ArgumentNullException> (() => list.RemoveAll ((string) null));

			// RemoveAt
			Assert.Throws<ArgumentOutOfRangeException> (() => list.RemoveAt (-1));

			// Replace
			Assert.Throws<ArgumentNullException> (() => list.Replace (null));
			Assert.Throws<ArgumentOutOfRangeException> (() => list.Replace (HeaderId.Unknown, "value"));
			Assert.Throws<ArgumentNullException> (() => list.Replace (HeaderId.AdHoc, null));
			Assert.Throws<ArgumentNullException> (() => list.Replace (null, "value"));
			Assert.Throws<ArgumentNullException> (() => list.Replace ("field", null));
			Assert.Throws<ArgumentOutOfRangeException> (() => list.Replace (HeaderId.Unknown, Encoding.UTF8, "value"));
			Assert.Throws<ArgumentNullException> (() => list.Replace (HeaderId.AdHoc, null, "value"));
			Assert.Throws<ArgumentNullException> (() => list.Replace (HeaderId.AdHoc, Encoding.UTF8, null));
			Assert.Throws<ArgumentNullException> (() => list.Replace (null, Encoding.UTF8, "value"));
			Assert.Throws<ArgumentNullException> (() => list.Replace ("field", null, "value"));
			Assert.Throws<ArgumentNullException> (() => list.Replace ("field", Encoding.UTF8, null));

			// WriteTo
			using (var stream = new MemoryStream ()) {
				Assert.Throws<ArgumentNullException> (() => list.WriteTo (FormatOptions.Default, null));
				Assert.Throws<ArgumentNullException> (() => list.WriteTo (null, stream));
				Assert.Throws<ArgumentNullException> (() => list.WriteTo (null));
			}

			// Indexers
			Assert.Throws<ArgumentOutOfRangeException> (() => list[-1] = new Header (HeaderId.AdHoc, "value"));
			Assert.Throws<ArgumentOutOfRangeException> (() => list[HeaderId.Unknown] = "value");
			Assert.Throws<ArgumentOutOfRangeException> (() => value = list[HeaderId.Unknown]);
			Assert.Throws<ArgumentOutOfRangeException> (() => header = list[-1]);
			Assert.Throws<ArgumentNullException> (() => value = list[null]);
			Assert.Throws<ArgumentNullException> (() => list[null] = "value");
			Assert.Throws<ArgumentNullException> (() => list[0] = null);
		}

		[Test]
		public void TestReplacingHeaders ()
		{
			const string ReplacedContentType = "text/plain; charset=iso-8859-1; name=body.txt";
			const string ReplacedContentDisposition = "inline; filename=body.txt";
			const string ReplacedContentLocation = "http://www.example.com/location";
			const string ReplacedContentId = "<content.id.2@localhost>";
			var headers = new HeaderList ();

			headers.Add (HeaderId.ContentId, "<content-id.1@localhost>");
			headers.Add ("Content-Location", "http://www.location.com");
			headers.Insert (0, HeaderId.ContentDisposition, "attachment");
			headers.Insert (0, "Content-Type", "text/plain");

			Assert.IsTrue (headers.Contains (HeaderId.ContentType), "Expected the list of headers to contain HeaderId.ContentType.");
			Assert.IsTrue (headers.Contains ("Content-Type"), "Expected the list of headers to contain a Content-Type header.");
			Assert.AreEqual (0, headers.LastIndexOf (HeaderId.ContentType), "Expected the Content-Type header to be the first header.");

			headers.Replace ("Content-Disposition", ReplacedContentDisposition);
			Assert.AreEqual (4, headers.Count, "Unexpected number of headers after replacing Content-Disposition.");
			Assert.AreEqual (ReplacedContentDisposition, headers["Content-Disposition"], "Content-Disposition has unexpected value after replacing it.");
			Assert.AreEqual (1, headers.IndexOf ("Content-Disposition"), "Replaced Content-Disposition not in the expected position.");

			headers.Replace (HeaderId.ContentType, ReplacedContentType);
			Assert.AreEqual (4, headers.Count, "Unexpected number of headers after replacing Content-Type.");
			Assert.AreEqual (ReplacedContentType, headers["Content-Type"], "Content-Type has unexpected value after replacing it.");
			Assert.AreEqual (0, headers.IndexOf ("Content-Type"), "Replaced Content-Type not in the expected position.");

			headers.Replace (HeaderId.ContentId, Encoding.UTF8, ReplacedContentId);
			Assert.AreEqual (4, headers.Count, "Unexpected number of headers after replacing Content-Id.");
			Assert.AreEqual (ReplacedContentId, headers["Content-Id"], "Content-Id has unexpected value after replacing it.");
			Assert.AreEqual (2, headers.IndexOf ("Content-Id"), "Replaced Content-Id not in the expected position.");

			headers.Replace ("Content-Location", Encoding.UTF8, ReplacedContentLocation);
			Assert.AreEqual (4, headers.Count, "Unexpected number of headers after replacing Content-Location.");
			Assert.AreEqual (ReplacedContentLocation, headers["Content-Location"], "Content-Location has unexpected value after replacing it.");
			Assert.AreEqual (3, headers.IndexOf ("Content-Location"), "Replaced Content-Location not in the expected position.");

			headers.RemoveAll ("Content-Location");
			Assert.AreEqual (3, headers.Count, "Unexpected number of headers after removing Content-Location.");
		}

		[Test]
		public void TestReplacingMultipleHeaders ()
		{
			const string CombinedRecpients = "first@localhost, second@localhost, third@localhost";
			var headers = new HeaderList ();

			headers.Add ("From", "sender@localhost");
			headers.Add ("To", "first@localhost");
			headers.Add ("To", "second@localhost");
			headers.Add ("To", "third@localhost");
			headers.Add ("Cc", "carbon.copy@localhost");

			headers.Replace ("To", CombinedRecpients);
			Assert.AreEqual (3, headers.Count, "Unexpected number of headers after replacing To header.");
			Assert.AreEqual (CombinedRecpients, headers["To"], "To header has unexpected value after being replaced.");
			Assert.AreEqual (1, headers.IndexOf ("To"), "Replaced To header not in the expected position.");
			Assert.AreEqual (0, headers.IndexOf ("From"), "From header not in the expected position.");
			Assert.AreEqual (2, headers.IndexOf ("Cc"), "Cc header not in the expected position.");
		}
	}
}
