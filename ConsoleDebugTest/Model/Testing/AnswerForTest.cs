﻿using System;

namespace TestingProgram.Model.Testing
{
	public class AnswerForTest
	{
		public enum AnswerType : byte
		{
			Bad,
			Good,
			Unresolved
		}
		public AnswerForTest(string text)
		{

		}
		public AnswerForTest(string login, DateTime start, DateTime end, int good, int bad, int unresolved, int all)
		{
			Login = login ?? throw new ArgumentNullException(nameof(login));
			Start = start;
			End = end;
			Good = good;
			Bad = bad;
			Unresolved = unresolved;
			All = all;
		}
		public AnswerForTest(string login, string ip, DateTime start, int all)
		{
			Login = login ?? throw new ArgumentNullException(nameof(login));
			Ip = ip ?? throw new ArgumentNullException(nameof(ip));
			Start = start;
			Good = 0;
			Bad = 0;
			Unresolved = 0;
			All = all;
		}

		public void AddAnswer(AnswerType type)
		{
			switch (type)
			{
				case AnswerType.Bad:
					Bad++;
					break;
				case AnswerType.Good:
					Good++;
					break;
				case AnswerType.Unresolved:
					Unresolved++;
					break;
			}
		}

		public string Login { get; private set; }
		public string Ip { get; set; }
		public DateTime Start { get; private set; }
		public DateTime End { get; set; }
		public int Good { get; private set; } = 0;
		public int Bad { get; private set; } = 0;
		public int Unresolved { get; private set; } = 0;
		public int All { get; private set; } = 0;

		public override string ToString()
			=> $"{Login}|{Ip}|{Start}|{End}|{Good}|{Bad}|{Unresolved}|{All}";		
	}
}
