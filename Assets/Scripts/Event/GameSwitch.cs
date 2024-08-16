public enum GameSwitch
{
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

    // 필요에 따라 더 추가
    NowDay2,

    Day2OnLever

}