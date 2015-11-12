using UnityEngine;
using System.Collections;

namespace CFCore
{
	/// <summary>
	/// 게임 세션.
	/// </summary>
	public class Session
	{

		// Members

		/// <summary>
		/// 현재 턴 (1턴 = 1주)
		/// </summary>
		public int currentTurn { get; private set; }
		/// <summary>
		/// 최대 턴, 이 제한선에 걸리면 강제로 엔딩 체크를 하게 된다
		/// 현재는 144턴 고정 ( = 36개월 = 3년 )
		/// </summary>
		public int maxTurn { get { return 144; } }



		Player		m_player;				// 플레이어 오브젝트
		Resource	m_resource;				// 게임 내 운용 가능한 자원

		public Player.IStat playerStat { get { return m_player.stat; } }
	}
}
