using UnityEngine;
using System.Collections;


namespace CFCore
{
	/// <summary>
	/// 플레이어 정보
	/// </summary>
	public class Player
	{
		/// <summary>
		/// 스탯 이름
		/// </summary>
		public enum StatNames
		{
			rank,				// 계급

			stamina,			// 스태미너
			beauty,				// 용모
			elegance,			// 기품
			allure,				// 색기
			knowledge,			// 지식
			conversation,		// 화술
			arts,				// 예술적 소양
			benevolence,		// 자애
			stress,				// 스트레스
		}

		/// <summary>
		/// 스탯에 관한 인터페이스
		/// </summary>
		public interface IStat
		{
			/// <summary>
			/// 이름
			/// </summary>
			string name { get; }

			/// <summary>
			/// 계급
			/// </summary>
			int rank { get; }
			/// <summary>
			/// 스태미너
			/// </summary>
			int stamina { get; }
			/// <summary>
			/// 용모
			/// </summary>
			int beauty { get; }
			/// <summary>
			/// 기품
			/// </summary>
			int elegance { get; }
			/// <summary>
			/// 색기
			/// </summary>
			int allure { get; }
			/// <summary>
			/// 지식
			/// </summary>
			int knowledge { get; }
			/// <summary>
			/// 화술
			/// </summary>
			int conversation { get; }
			/// <summary>
			/// 예술적 소양
			/// </summary>
			int arts { get; }
			/// <summary>
			/// 자애
			/// </summary>
			int benevolence { get; }
			/// <summary>
			/// 스트레스
			/// </summary>
			int stress { get; }
		}

		private class Stat : IStat
		{
			// 인터페이스 구현

			public string name		{ get; set; }
			public int rank
			{
				get { return this[StatNames.rank]; }
				set { this[StatNames.rank] = value; }
			}
			public int stamina
			{
				get { return this[StatNames.stamina]; }
				set { this[StatNames.stamina] = value; }
			}
			public int beauty
			{
				get { return this[StatNames.beauty]; }
				set { this[StatNames.beauty] = value; }
			}
			public int elegance
			{
				get { return this[StatNames.elegance]; }
				set { this[StatNames.elegance] = value; }
			}
			public int allure
			{
				get { return this[StatNames.allure]; }
				set { this[StatNames.allure] = value; }
			}
			public int knowledge
			{
				get { return this[StatNames.knowledge]; }
				set { this[StatNames.knowledge] = value; }
			}
			public int conversation
			{
				get { return this[StatNames.conversation]; }
				set { this[StatNames.conversation] = value; }
			}
			public int arts
			{
				get { return this[StatNames.arts]; }
				set { this[StatNames.arts] = value; }
			}
			public int benevolence
			{
				get { return this[StatNames.benevolence]; }
				set { this[StatNames.benevolence] = value; }
			}
			public int stress
			{
				get { return this[StatNames.stress]; }
				set { this[StatNames.stress] = value; }
			}


			//

			int [] m_stats;			// 실제 스탯 배열


			/// <summary>
			/// 인덱서
			/// </summary>
			/// <param name="index"></param>
			/// <returns></returns>
			public int this[StatNames index]
			{
				get { return m_stats[(int)index]; }
				set { m_stats[(int)index] = value; }
			}

			public Stat()
			{
				// 스탯 배열 초기화
				m_stats	= new int[System.Enum.GetValues(typeof(StatNames)).Length];
			}
		}



		// Members

		Stat			m_stat;				// 플레이어 스탯


		/// <summary>
		/// 플레이어 스탯
		/// </summary>
		public IStat stat { get { return m_stat; } }
	}
}