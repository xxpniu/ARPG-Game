using System;
using GameLogic.Game.Elements;
using EngineCore;
using Layout.LayoutElements;
using Layout;
using GameLogic.Game.LayoutLogics;
using EngineCore.Simulater;
using Layout.AITree;
using UMath;

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
        TimeLine GetTimeLineByPath(string path);

        /// <summary>
        /// Gets the magic by key.
        /// </summary>
        /// <returns>The magic by key.</returns>
        /// <param name="key">Key.</param>
        MagicData GetMagicByKey(string key);

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
        IBattleCharacter CreateBattleCharacterView(string res, UVector3 pos, UVector3 forword);

        /// <summary>
        /// Creates the releaser view.
        /// </summary>
        /// <returns>The releaser view.</returns>
        /// <param name="releaser">Releaser.</param>
        /// <param name="targt">Targt.</param>
        /// <param name="targetPos">Target position.</param>
        IMagicReleaser CreateReleaserView(IBattleCharacter releaser, IBattleCharacter targt, UVector3? targetPos);

        /// <summary>
        /// Creates the particle player.
        /// </summary>
        /// <returns>The particle player.</returns>
        /// <param name="releaser">Releaser.</param>
        /// <param name="layout">Layout.</param>
        IParticlePlayer CreateParticlePlayer(IMagicReleaser releaser, ParticleLayout layout);

        /// <summary>
        /// Creates the missile.
        /// </summary>
        /// <returns>The missile.</returns>
        /// <param name="releaser">Releaser.</param>
        /// <param name="layout">Layout.</param>
        IBattleMissile CreateMissile(IMagicReleaser releaser, MissileLayout layout);

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

        /// <summary>
        /// Processes the damage.
        /// </summary>
        /// <param name="view1">View1.</param>
        /// <param name="view2">View2.</param>
        /// <param name="result">Result.</param>
        void ProcessDamage(IBattleCharacter view1, IBattleCharacter view2, DamageResult result);
    }
}

