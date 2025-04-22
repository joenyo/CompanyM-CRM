// Main application logic
document.addEventListener('DOMContentLoaded', function () {
    // DOM elements
    
    const caseListSection = document.getElementById('caseList');
    const caseFormSection = document.getElementById('caseForm');
    const customerFormSection = document.getElementById('customerForm');
    const caseDetailsSection = document.getElementById('caseDetails');
    const confirmDialog = document.getElementById('confirmDialog');

    const casesTableBody = document.getElementById('casesTableBody');
    const formTitle = document.getElementById('formTitle');
    const createEditCaseForm = document.getElementById('createEditCaseForm');
    const createCustomerForm = document.getElementById('createCustomerForm');
    const caseDetailsContent = document.getElementById('caseDetailsContent');

    const newCaseBtn = document.getElementById('newCaseBtn');
    const cancelBtn = document.getElementById('cancelBtn');
    const newCustomerBtn = document.getElementById('newCustomerBtn');
    const cancelCustomerBtn = document.getElementById('cancelCustomerBtn');
    const editCaseBtn = document.getElementById('editCaseBtn');
    const deleteCaseBtn = document.getElementById('deleteCaseBtn');
    const backToListBtn = document.getElementById('backToListBtn');
    const confirmDeleteBtn = document.getElementById('confirmDeleteBtn');
    const cancelDeleteBtn = document.getElementById('cancelDeleteBtn');

    const customerNameFilter = document.getElementById('customerNameFilter');
    const channelFilter = document.getElementById('channelFilter');
    const applyFilterBtn = document.getElementById('applyFilterBtn');
    const resetFilterBtn = document.getElementById('resetFilterBtn');

    // Form input elements
    const caseIdInput = document.getElementById('caseId');
    const customerIdSelect = document.getElementById('customerId');
    const channelIdSelect = document.getElementById('channelId');
    const subjectInput = document.getElementById('subject');
    const descriptionInput = document.getElementById('description');
    const statusSelect = document.getElementById('status');

    const customerNameInput = document.getElementById('customerName');
    const emailInput = document.getElementById('email');
    const phoneInput = document.getElementById('phone');

    // State variables
    let currentCaseId = null;
    let cases = [];
    let channelTypes = [];
    let customers = [];

    // Initialize the application
    function init() {
        // Hide confirmation dialog explicitly
        if (confirmDialog) {
            confirmDialog.style.display = 'none';
        }
        
        // Show the case list section initially
        showSection(caseListSection);

        loadChannelTypes();
        loadCustomers();
        loadCases();
        bindEvents();
    }

    // Load channel types from API
    async function loadChannelTypes() {
        try {
            console.log('Fetching channel types from:', channelService.apiUrl);
            channelTypes = await channelService.getAllChannels();
            console.log('Channel Types:', channelTypes);
            populateChannelDropdowns();
        } catch (error) {
            console.error('Error fetching channel types:', error);
            alert('Failed to load channel types. Please try refreshing the page.');
        }
    }

    // Load customers from API
    async function loadCustomers() {
        try {
            console.log('Fetching customers from:', customerService.apiUrl);
            customers = await customerService.getAllCustomers();
            console.log('Customers:', customers);
            populateCustomerDropdown();
        } catch (error) {
            console.error('Error fetching customers:', error);
            alert('Failed to load customers. Please try refreshing the page.');
        }
    }

    // Load cases from API
    async function loadCases() {
        try {
            console.log('Fetching cases from:', caseService.apiUrl);
            cases = await caseService.getAllCases();
            console.log('Cases:', cases);
            renderCasesTable(cases);
        } catch (error) {
            console.error('Error fetching cases:', error);
            alert('Failed to load cases. Please try refreshing the page.');
        }
    }

    // Populate channel dropdowns (filter and form)
    function populateChannelDropdowns() {
        // For filter
        channelFilter.innerHTML = '<option value="">All Channels</option>';

        // For form
        channelIdSelect.innerHTML = '';

        // Check if channelTypes has a $values property (from ReferenceHandler.Preserve)
        const channelsArray = channelTypes.$values || channelTypes;

        if (Array.isArray(channelsArray)) {
            channelsArray.forEach(channel => {
                // Add to filter dropdown
                const filterOption = document.createElement('option');
                filterOption.value = channel.channelID;
                filterOption.textContent = channel.channelName;
                channelFilter.appendChild(filterOption);

                // Add to form dropdown
                const formOption = document.createElement('option');
                formOption.value = channel.channelID;
                formOption.textContent = channel.channelName;
                channelIdSelect.appendChild(formOption);
            });
        } else {
            console.error('Channel types is not an array:', channelTypes);
        }
    }

    // Populate customer dropdown
    function populateCustomerDropdown() {
        customerIdSelect.innerHTML = '';

        // Check if customers has a $values property
        const customersArray = customers.$values || customers;

        if (Array.isArray(customersArray)) {
            customersArray.forEach(customer => {
                const option = document.createElement('option');
                option.value = customer.customerID;
                option.textContent = customer.customerName;
                customerIdSelect.appendChild(option);
            });
        } else {
            console.error('Customers is not an array:', customers);
        }
    }

    // Render cases table
    function renderCasesTable(casesToRender) {
        casesTableBody.innerHTML = '';
        console.log("Raw data:", casesToRender);
    
        // Check if casesToRender has a $values property
        let casesArray = casesToRender.$values || casesToRender;
        
        // Create a map of all objects by their $id
        const objectsById = {};
        
        // Function to collect all objects with $id in the response
        function collectObjectsById(obj) {
            if (!obj || typeof obj !== 'object') return;
            
            if (obj.$id) {
                objectsById[obj.$id] = obj;
            }
            
            for (const key in obj) {
                if (obj[key] && typeof obj[key] === 'object') {
                    collectObjectsById(obj[key]);
                }
            }
        }
        
        // Collect all objects with IDs
        collectObjectsById(casesToRender);
        
        // Function to resolve references
        function resolveReference(item) {
            if (item && item.$ref && objectsById[item.$ref]) {
                return objectsById[item.$ref];
            }
            return item;
        }
        
        // Process the array to resolve references
        const processedCases = [];
        for (let i = 0; i < casesArray.length; i++) {
            let caseItem = casesArray[i];
            
            // If it's a reference, resolve it
            if (caseItem.$ref) {
                caseItem = resolveReference(caseItem);
            }
            
            // Only add valid case items
            if (caseItem && caseItem.caseID) {
                processedCases.push(caseItem);
            }
        }
        
        console.log("Processed cases:", processedCases);
        
        if (processedCases.length === 0) {
            const row = document.createElement('tr');
            row.innerHTML = '<td colspan="7" class="text-center">No cases found</td>';
            casesTableBody.appendChild(row);
            return;
        }
    
        processedCases.forEach(caseItem => {
            const row = document.createElement('tr');
    
            // Get channel name
            const channelName = caseItem.channel
                ? caseItem.channel.channelName
                : getChannelNameById(caseItem.channelID);
    
            // Format date
            const createdDate = new Date(caseItem.createdDate).toLocaleDateString();
    
            row.innerHTML = `
                <td>${caseItem.caseID}</td>
                <td>${caseItem.customerName || 'Unknown'}</td>
                <td>${channelName}</td>
                <td>${caseItem.subject}</td>
                <td>${caseItem.status}</td>
                <td>${createdDate}</td>
                <td>
                    <button class="btn btn-small view-case" data-id="${caseItem.caseID}">View</button>
                    <button class="btn btn-small edit-case" data-id="${caseItem.caseID}">Edit</button>
                    <button class="btn btn-small btn-danger delete-case" data-id="${caseItem.caseID}">Delete</button>
                </td>
            `;
    
            casesTableBody.appendChild(row);
        });
    
        // Add event listeners to the buttons
        document.querySelectorAll('.view-case').forEach(btn => {
            btn.addEventListener('click', function () {
                const caseId = parseInt(this.getAttribute('data-id'));
                viewCaseDetails(caseId);
            });
        });
    
        document.querySelectorAll('.edit-case').forEach(btn => {
            btn.addEventListener('click', function () {
                const caseId = parseInt(this.getAttribute('data-id'));
                openEditCaseForm(caseId);
            });
        });
    
        document.querySelectorAll('.delete-case').forEach(btn => {
            btn.addEventListener('click', function (event) {
                event.preventDefault();
                const caseId = parseInt(this.getAttribute('data-id'));
                console.log("Delete button clicked for case:", caseId);
                openDeleteConfirmation(caseId);
            });
        });
    }

    // Get channel name by ID
    function getChannelNameById(channelId) {
        // Check if channelTypes has a $values property
        const channelsArray = channelTypes.$values || channelTypes;

        if (Array.isArray(channelsArray)) {
            const channel = channelsArray.find(c => c.channelID === channelId);
            return channel ? channel.channelName : 'Unknown';
        }
        return 'Unknown';
    }

    // View case details
    async function viewCaseDetails(caseId) {
        try {
            const caseData = await caseService.getCaseById(caseId);
            currentCaseId = caseId;

            const channelName = caseData.channel
                ? caseData.channel.channelName
                : getChannelNameById(caseData.channelID);

            const createdDate = new Date(caseData.createdDate).toLocaleDateString();
            const updatedDate = new Date(caseData.lastUpdatedDate).toLocaleDateString();

            caseDetailsContent.innerHTML = `
                <div class="case-detail">
                    <strong>Customer:</strong> ${caseData.customer ? caseData.customer.customerName : 'Unknown'}
                </div>
                <div class="case-detail">
                    <strong>Channel:</strong> ${channelName}
                </div>
                <div class="case-detail">
                    <strong>Subject:</strong> ${caseData.subject}
                </div>
                <div class="case-detail">
                    <strong>Description:</strong> ${caseData.description || 'No description provided'}
                </div>
                <div class="case-detail">
                    <strong>Status:</strong> ${caseData.status}
                </div>
                <div class="case-detail">
                    <strong>Created:</strong> ${createdDate}
                </div>
                <div class="case-detail">
                    <strong>Last Updated:</strong> ${updatedDate}
                </div>
            `;

            showSection(caseDetailsSection);
        } catch (error) {
            console.error('Error viewing case details:', error);
            alert('Failed to load case details.');
        }
    }

    // Open the form to create a new case
    function openNewCaseForm() {
        formTitle.textContent = 'New Case';
        createEditCaseForm.reset();
        caseIdInput.value = '';
        currentCaseId = null;
        showSection(caseFormSection);
    }

    // Open the form to edit an existing case
    async function openEditCaseForm(caseId) {
        try {
            const caseData = await caseService.getCaseById(caseId);
            currentCaseId = caseId;

            formTitle.textContent = 'Edit Case';
            caseIdInput.value = caseData.caseID;
            customerIdSelect.value = caseData.customerID;
            channelIdSelect.value = caseData.channelID;
            subjectInput.value = caseData.subject;
            descriptionInput.value = caseData.description || '';
            statusSelect.value = caseData.status;

            showSection(caseFormSection);
        } catch (error) {
            console.error('Error editing case:', error);
            alert('Failed to load case for editing.');
        }
    }

    // Open customer form
    function openCustomerForm() {
        createCustomerForm.reset();
        showSection(customerFormSection);
    }

    // Open delete confirmation dialog
    function openDeleteConfirmation(caseId) {
        currentCaseId = caseId;
        const dialog = document.getElementById('confirmDialog');
        
        // Make dialog visible first with opacity 0
        dialog.style.display = 'flex';
        
        // Force a reflow to enable transition
        void dialog.offsetWidth;
        
        // Add visible class to trigger animation
        dialog.classList.add('visible');
        
        console.log("Opening delete confirmation for case:", caseId);
    }

    // Close delete confirmation dialog
    function closeDeleteConfirmation() {
        const dialog = document.getElementById('confirmDialog');
        
        // Remove visible class to start fade out
        dialog.classList.remove('visible');
        
        // Wait for animation to complete before hiding
        setTimeout(() => {
            dialog.style.display = 'none';
        }, 300); // Match the CSS transition duration
    }

    // Show a specific section and hide others
    function showSection(sectionToShow) {
        // First hide all sections
        caseListSection.classList.add('hidden');
        caseFormSection.classList.add('hidden');
        customerFormSection.classList.add('hidden');
        caseDetailsSection.classList.add('hidden');
        
        // Then show the desired section with animation
        sectionToShow.classList.remove('hidden');
    }

    // Handle search/filter
    async function handleSearch() {
        const nameFilter = customerNameFilter.value.trim();
        const chFilter = channelFilter.value;

        try {
            if (!nameFilter && !chFilter) {
                await loadCases();
                return;
            }

            const filteredCases = await caseService.searchCases(
                nameFilter || null,
                chFilter || null
            );

            renderCasesTable(filteredCases);
        } catch (error) {
            console.error('Error searching cases:', error);
            alert('Failed to search cases.');
        }
    }

    // Reset filters
    function resetFilters() {
        customerNameFilter.value = '';
        channelFilter.value = '';
        loadCases();
    }

    // Save case (create or update)
    async function saveCase(event) {
        event.preventDefault();

        const caseData = {
            customerId: parseInt(customerIdSelect.value),
            channelId: parseInt(channelIdSelect.value),
            subject: subjectInput.value,
            description: descriptionInput.value,
            status: statusSelect.value
        };

        try {
            if (currentCaseId) {
                // Update existing case
                caseData.caseID = currentCaseId;
                await caseService.updateCase(currentCaseId, caseData);
                alert('Case updated successfully.');
            } else {
                // Create new case
                await caseService.createCase(caseData);
                alert('Case created successfully.');
            }

            await loadCases();
            showSection(caseListSection);
        } catch (error) {
            console.error('Error saving case:', error);
            alert('Failed to save case.');
        }
    }

    // Save customer
    async function saveCustomer(event) {
        event.preventDefault();

        const customerData = {
            customerName: customerNameInput.value,
            email: emailInput.value,
            phone: phoneInput.value
        };

        try {
            const newCustomer = await customerService.createCustomer(customerData);
            alert('Customer created successfully.');

            // Refresh customer list
            await loadCustomers();

            // Return to case form
            showSection(caseFormSection);

            // Select the newly created customer
            customerIdSelect.value = newCustomer.customerID;
        } catch (error) {
            console.error('Error creating customer:', error);
            alert('Failed to create customer.');
        }
    }

    // Delete case
    async function deleteCase() {
        try {
            await caseService.deleteCase(currentCaseId);
            alert('Case deleted successfully.');
            closeDeleteConfirmation();
            await loadCases();
            showSection(caseListSection);
        } catch (error) {
            console.error('Error deleting case:', error);
            alert('Failed to delete case.');
        }
    }

    // Bind all event listeners
    function bindEvents() {
        // Navigation buttons
        newCaseBtn.addEventListener('click', openNewCaseForm);
        cancelBtn.addEventListener('click', () => showSection(caseListSection));
        newCustomerBtn.addEventListener('click', openCustomerForm);
        cancelCustomerBtn.addEventListener('click', () => showSection(caseFormSection));
        backToListBtn.addEventListener('click', () => showSection(caseListSection));

        // Case details actions
        editCaseBtn.addEventListener('click', () => openEditCaseForm(currentCaseId));
        deleteCaseBtn.addEventListener('click', () => openDeleteConfirmation(currentCaseId));

        // Delete confirmation - adding null checks
        if (confirmDeleteBtn) {
            confirmDeleteBtn.addEventListener('click', deleteCase);
        }
        
        if (cancelDeleteBtn) {
            cancelDeleteBtn.addEventListener('click', closeDeleteConfirmation);
        }

        // Form submissions
        createEditCaseForm.addEventListener('submit', saveCase);
        createCustomerForm.addEventListener('submit', saveCustomer);

        // Filter actions
        applyFilterBtn.addEventListener('click', handleSearch);
        resetFilterBtn.addEventListener('click', resetFilters);
    }

    // Start the application
    init();
});