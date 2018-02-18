$(document).ready(function (){
    let address = "0xe78a0f7e598cc8b0bb87894b0f60dd2a88d6a8ab"
    let abi = [
        {
            "constant": true,
            "inputs": [
                {
                    "name": "hash",
                    "type": "string"
                }
            ],
            "name": "vefiry",
            "outputs": [
                {
                    "name": "dateCreated",
                    "type": "uint256"
                }
            ],
            "payable": false,
            "stateMutability": "view",
            "type": "function"
        },
        {
            "constant": false,
            "inputs": [
                {
                    "name": "hash",
                    "type": "string"
                }
            ],
            "name": "add",
            "outputs": [
                {
                    "name": "dateCreated",
                    "type": "uint256"
                }
            ],
            "payable": false,
            "stateMutability": "nonpayable",
            "type": "function"
        },
        {
            "inputs": [],
            "payable": false,
            "stateMutability": "nonpayable",
            "type": "constructor"
        }
    ];

    $('#linkHome').click(function () {
        showView("viewHome")
    });
    $('#linkSubmitDocument').click(function () {
        showView("viewSubmitDocument")
    });
    $('#linkVerifyDocument').click(function () {
        showView("viewVerifyDocument")
    });
    $('#documentUploadButton').click(uploadDocument);
    $('#documentVerifyButton').click(verifyDocument);
    // Attach AJAX "loading" event listener
    $(document).on({
        ajaxStart: function () {
            $("#loadingBox").show()
        },
        ajaxStop: function () {
            $("#loadingBox").hide()
        }
    });
    
    function showView(viewName) {
        // Hide all views and show the selected view only
        $('main > section').hide();
        $('#' + viewName).show();
    }
    
    function showInfo(message) {
        $('#infoBox>p').html(message);
        $('#infoBox').show();
        $('#infoBox>header').click(function () {
            $('#infoBox').hide();
        });
    }
    
    function showError(errorMsg) {
        $('#errorBox>p').html("Error: " + errorMsg);
        $('#errorBox').show();
        $('#errorBox>header').click(function () {
            $('#errorBox').hide();
        });
    }
    
    function uploadDocument() {
        if ($('#documentForUpload')[0].files.length == 0) {
            return showError("Please select a file to upload");
        }

        let fileReader = new FileReader();
        fileReader.onload = function() {
            let docHash = sha256(fileReader.result);
            if (typeof web3 === 'undefined'){
                return showError("Please install MetaMask");
            }

            let contract = web3.eth.contract(abi).at(address);
            contract.add(docHash, function(err, result) {
                if(err) {
                    return showError("Smaert contract call failed: " + err)
                }

                showInfo('Document ${docHash} successfully added');
            })
        }

        fileReader.readAsBinaryString($('#documentForUpload')[0].files[0]);
    }

    function verifyDocument() {
        if ($('#documentToVerify')[0].files.length == 0) {
            return showError("Please select a file to upload");
        }

        let fileReader = new FileReader();
        fileReader.onload = function() {
            let docHash = sha256(fileReader.result);
            if (typeof web3 === 'undefined'){
                return showError("Please install MetaMask");
            }

            let contract = web3.eth.contract(abi).at(address);
            contract.vefiry(docHash, function(err, result) {
                if(err) {
                    return showError("Smaert contract call failed: " + err)
                }

                let date = result.c;
                if (date > 0) {
                    let displayDate = new Date(date * 1000).toLocaleString();
                    showInfo('Document date ' + displayDate);
                } else {
                    showError("Invalid document");
                }
            })
        }

        fileReader.readAsBinaryString($('#documentToVerify')[0].files[0]);
    }
})