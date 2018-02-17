pragma solidity ^0.4.18;

contract TokenSharesContract {
    address private owner;
    uint private price;
    mapping (address=>uint) private shares;
    address[] private shareholders;
    mapping (address=>bool) private allowedToWithdrawFunds;

    modifier isOwner() {
        require(owner == msg.sender);
        _;
    }

    function TokenSharesContract(uint initialSupply) public {
        owner = msg.sender;
        shares[msg.sender] = initialSupply;
        allowedToWithdrawFunds[msg.sender] = true;
        price = 1 ether;
    }

    function getPricePerShare() public view returns(uint) {
        return price / 1 ether;
    }

    function calcTransWorth(uint amount) public view returns(uint) {
        return ((amount / 1 ether) * price) / 1 ether; 
    }

    function buyShares() public payable {
        uint sharesToBuy = calcTransWorth(msg.value);
        require(shares[owner] >= sharesToBuy);
        require(shares[msg.sender] + sharesToBuy > shares[msg.sender]);

        shares[owner] -= sharesToBuy;
        shares[msg.sender] += sharesToBuy;
        shareholders.push(msg.sender);
    }

    function getShareholders() isOwner public view returns(address[]) {
        return shareholders;
    }

     function allowWithdrawal(address addr) isOwner public {
        allowedToWithdrawFunds[addr] = true;
    }

    function deposit() isOwner public payable {
        
    }

    function getBalance() isOwner public view returns(uint) {
        return this.balance / 1 ether;
    }

    function getNumberOfShares(address addr) public view returns(uint) {
        require(shares[msg.sender] > 0);
        return shares[addr];
    }

    function withdrawal(uint amount) public {
        require(shares[msg.sender] > 0);
        require(allowedToWithdrawFunds[msg.sender] == true);
        uint amountInEther = amount * 1 ether;
        require(this.balance >= amountInEther);
        msg.sender.transfer(amountInEther);
    }
}