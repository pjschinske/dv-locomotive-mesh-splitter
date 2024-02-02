using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;

namespace Dummiesman {
	internal class CharWordReader {
		internal char[] word;
		internal int wordSize;
		internal bool endReached;

		private StreamReader reader;
		private int bufferSize;
		private char[] buffer;
		
		internal char currentChar;
		private int currentPosition = 0;
		private int maxPosition = 0;

		internal CharWordReader(StreamReader reader, int bufferSize) {
			this.reader = reader;
			this.bufferSize = bufferSize;

			this.buffer = new char[this.bufferSize];
			this.word = new char[this.bufferSize];

			this.MoveNext();
		}

		internal void SkipWhitespaces() {
			while (char.IsWhiteSpace(this.currentChar)) {
				this.MoveNext();
			}
		}

		internal void SkipWhitespaces(out bool newLinePassed) {
			newLinePassed = false;
			while (char.IsWhiteSpace(this.currentChar)) {
				if (this.currentChar == '\r' || this.currentChar == '\n') {
					newLinePassed = true;
				}
				this.MoveNext();
			}
		}

		internal void SkipUntilNewLine() {
			while (this.currentChar != char.MinValue && this.currentChar != '\n' && this.currentChar != '\r') {
				this.MoveNext();
			}
			this.SkipNewLineSymbols();
		}

		internal void ReadUntilWhiteSpace() {
			this.wordSize = 0;
			while (this.currentChar != char.MinValue && char.IsWhiteSpace(this.currentChar) == false) {
				this.word[this.wordSize] = this.currentChar;
				this.wordSize++;
				this.MoveNext(); 
			}
		}

		internal void ReadUntilNewLine() {
			this.wordSize = 0;
			while (this.currentChar != char.MinValue && this.currentChar != '\n' && this.currentChar != '\r') {
				this.word[this.wordSize] = this.currentChar;
				this.wordSize++;
				this.MoveNext();
			}
			this.SkipNewLineSymbols();
		}

		internal bool Is(string other) {
			if (other.Length != this.wordSize) {
				return false;
			}

			for (int i=0; i<this.wordSize; i++) {
				if (this.word[i] != other[i]) {
					return false;
				}
			}

			return true;
		}
        internal string GetString(int startIndex = 0) {
            if (startIndex >= this.wordSize - 1) {
                return string.Empty;
            }
            return new string(this.word, startIndex, this.wordSize - startIndex);
        }

		internal Vector3 ReadVector() {
			this.SkipWhitespaces();
			float x = this.ReadFloat();
			this.SkipWhitespaces();
			float y = this.ReadFloat();
			this.SkipWhitespaces(out var newLinePassed);
			float z = 0f;
			if (newLinePassed == false) {
				z = this.ReadFloat();
			}
			return new Vector3(x, y, z);
		}

		internal int ReadInt() {
			int result = 0;
			bool isNegative = this.currentChar == '-';
			if (isNegative == true) {
				this.MoveNext();
			}
			
			while (this.currentChar >= '0' && this.currentChar <= '9') {
				var digit = this.currentChar - '0';
				result = result * 10 + digit;
				this.MoveNext();
			}

			return (isNegative == true) ? -result : result;
		}

		internal float ReadFloat() {
			bool isNegative = this.currentChar == '-';
			if (isNegative) {
				this.MoveNext();
			}

			var num = (float)this.ReadInt();
			if (this.currentChar == '.' || this.currentChar == ',') {
				this.MoveNext();
				num +=  this.ReadFloatEnd();

				if (this.currentChar == 'e' || this.currentChar == 'E') {
					this.MoveNext();
					var exp = this.ReadInt();
					num = num * Mathf.Pow(10f, exp);
				}
			}
			if (isNegative == true) {
				num = -num;
			}

			return num;
		}

		private float ReadFloatEnd() {
			float result = 0f;

			var exp = 0.1f;
			while (this.currentChar >= '0' && this.currentChar <= '9') {
				var digit = this.currentChar - '0';
				result += digit * exp;

				exp *= 0.1f;

				this.MoveNext();
			}

			return result;
		}

		private void SkipNewLineSymbols() {
			while (this.currentChar == '\n' || this.currentChar == '\r') {
				this.MoveNext();
			}
		}

		internal void MoveNext() {
			this.currentPosition++;
			if (this.currentPosition >= this.maxPosition) {
				if (this.reader.EndOfStream == true) {
					this.currentChar = char.MinValue;
					this.endReached = true;
					return;
				}

				this.currentPosition = 0;
				this.maxPosition = this.reader.Read(this.buffer, 0, this.bufferSize);
			}
			this.currentChar = this.buffer[this.currentPosition];
		}
	}
}
