pragma solidity ^0.4.18;

contract SimpleTimedAuction {
    address private owner;
    uint private endBlock;
    mapping (address=>uint) private bids;
    mapping (address=>uint) private tokenOwners;
    address private highterBid;
    uint private highterAmount;

    event Bid(address, uint);

    modifier isOwner() {
        require(owner == msg.sender);
        _;
    }

     modifier isOver() {
        require(block.number >= endBlock);
        _;
    }

     modifier isWinner() {
        require(highterBid == msg.sender);
        _;
    }

    function SimpleTimedAuction(uint tokens) public {
        owner = msg.sender;
        endBlock = block.number + 1;
        tokenOwners[msg.sender] = tokens;
    }

    function bid(uint amount) public {
        require(block.number < endBlock);

        Bid(msg.sender, amount);
        bids[msg.sender] = amount;

        if (amount > highterAmount) {
            highterAmount = amount;
            highterBid = msg.sender;
        }
    }

    function pay() isOver isWinner public payable {
        require(msg.value == highterAmount);
        uint tokensAmount = tokenOwners[owner];
        tokenOwners[msg.sender] = tokensAmount;
        tokenOwners[owner] = 0;
    }

    function getWinner() isOver public view returns (address, uint) {
        return (highterBid, highterAmount);
    }

    function getEndBlock() public view returns(uint) {
        return endBlock;
    }

    function getTokens() public view returns(uint) {
        return tokenOwners[msg.sender];
    }
}