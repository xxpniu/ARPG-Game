using System;
using GameLogic.Game.Elements;
using EngineCore;
using Layout.LayoutElements;
using Layout;
using GameLogic.Game.LayoutLogics;

namespace GameLogic.Game.Perceptions
{
	/// <summary>
	/// I battle perception.
	/// </summary>
	public interface IBattlePerception
	{
		/// <summary>
		/// 获取当前的layout
		/// </summary>
		/// <returns>The time line by path.</returns>
		/// <param name="path">Path.</param>
		TimeLine GetTimeLineByPath (string path);
		/// <summary>
		/// Gets the magic by key.
		/// </summary>
		/// <returns>The magic by key.</returns>
		/// <param name="key">Key.</param>
		MagicData GetMagicByKey (string key);

		/// <summary>
		/// Creates the battle character view.
		/// </summary>
		/// <returns>The battle character view.</returns>
		IBattleCharacter CreateBattleCharacterView();

		/// <summary>
		/// Creates the releaser view.
		/// </summary>
		/// <returns>The releaser view.</returns>
		/// <param name="releaser">Releaser.</param>
		/// <param name="targt">Targt.</param>
		/// <param name="targetPos">Target position.</param>
		IMagicReleaser CreateReleaserView (IBattleCharacter releaser,IBattleCharacter targt, GVector3? targetPos);

		/// <summary>
		/// Creates the particle player.
		/// </summary>
		/// <returns>The particle player.</returns>
		/// <param name="from">From.</param>
		/// <param name="fromBone">From bone.</param>
		/// <param name="to">To.</param>
		/// <param name="toBone">To bone.</param>
		IParticlePlayer CreateParticlePlayer (IBattleCharacter from, string fromBone, IBattleCharacter  to, string toBone);

		/// <summary>
		/// Distance the specified v and v2.
		/// </summary>
		/// <param name="v">V.</param>
		/// <param name="v2">V2.</param>
		float Distance (GVector3 v, GVector3 v2);

		/// <summary>
		/// Angle the specified v and v2.
		/// </summary>
		/// <param name="v">V.</param>
		/// <param name="v2">V2.</param>
		float Angle(GVector3 v,GVector3 v2);

		/// <summary>
		/// Rotates the with y.
		/// </summary>
		/// <returns>The with y.</returns>
		/// <param name="v">V.</param>
		/// <param name="angle">Angle.</param>
		GVector3 RotateWithY(GVector3 v, float angle);
	}
}

