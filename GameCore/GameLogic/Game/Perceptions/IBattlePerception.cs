using System;
using GameLogic.Game.Elements;
using EngineCore;
using Layout.LayoutElements;
using Layout;
using GameLogic.Game.LayoutLogics;
using EngineCore.Simulater;
using Layout.AITree;

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
		/// Exists the magic key.
		/// </summary>
		/// <returns>The magic key.</returns>
		/// <param name="key">Key.</param>
		bool ExistMagicKey(string key);
		/// <summary>
		/// Creates the battle character view.
		/// </summary>
		/// <returns>The battle character view.</returns>
		/// <param name="res">Res.</param>
		/// <param name="pos">Position.</param>
		/// <param name="forword">Forword.</param>
		IBattleCharacter CreateBattleCharacterView(string res, GVector3 pos, GVector3 forword);
        /// <summary>
        /// Normals the verctor.
        /// </summary>
        /// <returns>The verctor.</returns>
        /// <param name="gVector3">G vector3.</param>
		GVector3 NormalVerctor(GVector3 gVector3);

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
        /// <param name="releaser">Releaser.</param>
        /// <param name="layout">Layout.</param>
        IParticlePlayer CreateParticlePlayer (IMagicReleaser releaser,ParticleLayout layout );

		/// <summary>
		/// Creates the missile.
		/// </summary>
		/// <returns>The missile.</returns>
		/// <param name="releaser">Releaser.</param>
		/// <param name="layout">Layout.</param>
		IBattleMissile CreateMissile (IMagicReleaser releaser, MissileLayout layout);
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

		/// <summary>
		/// 当前的时间仿真
		/// </summary>
		/// <returns>The time simulater.</returns>
		ITimeSimulater GetTimeSimulater();
		/// <summary>
		/// Gets the AIT ree.
		/// </summary>
		/// <returns>The AIT ree.</returns>
		/// <param name="pathTree">Path tree.</param>
		TreeNode GetAITree(string pathTree);
	}
}

