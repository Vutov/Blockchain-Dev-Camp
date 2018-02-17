pragma solidity ^0.4.18;

contract EventCreationContract {
    event ShowAddress(address);

    function EventCreationContract() public {
        ShowAddress(msg.sender);
    }
}