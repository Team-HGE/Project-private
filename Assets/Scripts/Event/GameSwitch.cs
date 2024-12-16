public enum GameSwitch
{
    // 게임중인지
    IsPlayingGame,

    //스토리 관련
    isMainStoryOff,

    // 메인 전력
    isCentralPowerActive,

    // 중앙 제어실 카드
    hasSecurityCard,
    // 밤인지
    IsNight, 

    HasKey,

    // 태그 제외 전부 Lock
    DoorUnlocked,

    BossDefeated,

    Newtype,

    IsDaytime,

    IsTutorailEnd,

    //침대상호작용
    GoToBed, 

    // 바리게이트
    BarrierInteract, BarrierIsOpen,


    OneFloorOpenable, OneFloorStartEscape, OneFloorEncountAtA, OneFloorEndEscape,

    // 2일차인지
    NowDay2,
    // 2일차 레버를 올려 전력 복구
    Day_2_A2F_LeverOn,
    // 2일차 카드를 획득했는지
    Day2GetCardKey,
    // 2일차 컴퓨터 패스워드 힌트
    Day2GetPasswordHint,
}