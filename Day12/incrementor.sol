pragma solidity ^0.4.18;

contract IncrementorContract {

    uint256 private number;

    function get() public view returns (uint256) {
        return number;
    }

    function increment(uint256 delta) public {
        number += delta;
    }
}