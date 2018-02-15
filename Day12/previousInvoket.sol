pragma solidity ^0.4.18;

contract PreviousInvokerContract {

    address private previousInvoker;

    function getPreviousInvoker() public returns (address) {
        previousInvoker = msg.sender;
        return previousInvoker;
    }
}