pragma solidity ^0.4.18;

contract IndexedEvent {
    event ShowInformation(uint indexed price, uint indexed amount);

    function showInformation(uint price, uint amount) public {
        ShowInformation(price, amount);
    }
}