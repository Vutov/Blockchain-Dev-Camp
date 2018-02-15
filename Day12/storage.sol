pragma solidity ^0.4.18;

contract SimpleStorage {

    uint256 private storedData;

    function set(uint256 num) public {
        storedData = num;
    }
    
    function get() public constant returns(uint256 num) {
        return storedData;
    }
}
