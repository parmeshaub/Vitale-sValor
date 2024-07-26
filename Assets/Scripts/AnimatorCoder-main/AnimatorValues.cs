//Author: Small Hedge Games
//Date: 05/04/2024

namespace SHG.AnimatorCoder
{
    /// <summary> Complete list of all animation state names </summary>
    public enum Animations
    {
        IDLE,
        IDLE_COMBAT,
        DODGE_FRONT,
        DODGE_BACK,
        DODGE_COMBAT_FRONT,
        DODGE_COMBAT_BACK,
        DODGE_AIR_FRONT,
        DODGE_AIR_BACK,
        DODGE_AIR_COMBAT_FRONT,
        DODGE_AIR_COMBAT_BACK,
        WALK_START,
        WALK_LOOP,
        WALK_STOP,
        WALK_COMBAT_START,
        WALK_COMBAT_LOOP,
        WALK_COMBAT_STOP,
        RUN_START,
        RUN_LOOP,
        RUN_STOP,
        RUN_COMBAT_START,
        RUN_COMBAT_LOOP,
        RUN_COMBAT_STOP,
        SPRINT_START,
        SPRINT_LOOP,
        SPRINT_STOP,
        SPRINT_COMBAT_START,
        SPRINT_COMBAT_LOOP,
        SPRINT_COMBAT_STOP,
        JUMP_START,
        JUMP_LOOP,
        JUMP_END,
        JUMP_COMBAT_START,
        JUMP_COMBAT_LOOP,
        JUMP_COMBAT_END,
        IDLE_COMBAT_TO_IDLE,
        IDLE_TO_IDLE_COMBAT,
        WALK_TO_WALK_COMBAT,
        WALK_COMBAT_TO_WALK,
        SPRINT_COMBAT_TO_SPRINT,
        SPRINT_TO_SPRINT_COMBAT,
        PARRY,
        PARRY_COUNTER,
        HEAVY_ATTACK_01,
        HEAVY_ATTACK_02,
        HEAVY_ATTACK_03,
        HEAVY_ATTACK_04,
        LIGHT_ATTACK_02_01,
        LIGHT_ATTACK_02_02,
        LIGHT_ATTACK_02_03,
        LIGHT_ATTACK_02_04,
        LIGHT_ATTACK_01_01,
        LIGHT_ATTACK_01_02,
        LIGHT_ATTACK_01_03,
        LIGHT_ATTACK_01_04,
        BLOCK_START,
        BLOCK_LOOP,
        BLOCK_END,
        BLOCK_HIT,
        BLOCK_HIT_BREAK,
        GET_UP,
        GET_UP_COMBAT,
        ABLAZE,
        JUDGEMENT,
        SANCTURARY,
        WINGS_OF_COMFORT_START,
        GLIDE_LOOP,
        COMBUSTION,
        BLISTFULLNESS,
        GLACIATE,
        RAZOR_FANGS,
        VOLLEY,
        GELIDITY,
        NONE,
        RESET
    }

    /// <summary> Complete list of all animator parameters </summary>
    public enum Parameters
    {
        //Change the list below to your animator parameters
        GROUNDED,
        FALLING
    }
}


