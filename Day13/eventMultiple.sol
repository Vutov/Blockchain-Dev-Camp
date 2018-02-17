pragma solidity ^0.4.18;

contract EventMultipleContract {
    event ShowInformation(string, address);

    function showInformation(string data) public {
        ShowInformation(data, msg.sender);
    }
}