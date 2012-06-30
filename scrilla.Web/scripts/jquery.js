/// <reference path="https://ajax.googleapis.com/ajax/libs/jquery/1.6.3/jquery.min.js" />
/// <reference path="/scripts/jquery-ui-1.8.16.datepicker.min.js" />
/// <reference path="/scripts/jquery-ui-1.8.16.selectable.min.js" />

$(document).ready(function () {
	$('.chzn-select').chosen();

	$('h2 a').click(function () {
		$('form fieldset').toggle();
	});

	$('table.editable tr').dblclick(function () {
		edit($(this));
	});

	$('table.editable .edit-button').click(function () {
		var tr = $(this).parents('tr');
		edit(tr);
	});

	$('#vendor-mapping a.delete').click(function () {
		$(this).parents('tr').slideToggle();
	});

	function edit(tr) {
		$(tr).toggleClass('editing');

		var categoryId = $(tr).data('category-id');
		$(tr).find('.category select').each(function () {
			if (!$(this).hasClass('filled')) {
				$.tmpl("categoryTemplate", categories).appendTo($(this));
				$(this).addClass('filled');
			}
			$(this).find('option[value="' + categoryId + '"]').attr("selected", "selected");
		});

		var vendorId = $(tr).data('vendor-id');
		$(tr).find('.vendor select').each(function () {
			if (!$(this).hasClass('filled')) {
				$.tmpl("vendorTemplate", vendors).appendTo($(this));
				$(this).addClass('filled');
			}
			$(this).find('option[value="' + vendorId + '"]').attr("selected", "selected");
		});
	}

	$('.budget:not(.editing)').dblclick(function () {
		var td = $(this);
		$(this).find('.edit-mode input').each(function () {
			$(this).focus();
			$(td).toggleClass('editing');
		});
	});

	$('.budget input').blur(function () {
		var sel = $(this);
		var td = $(this).parents('td');
		var tr = $(this).parents('tr');
		var month = $(td).data('month');
		var catId = $(tr).data('category-id');
		var amount = $(this).val();

		$.ajax({
			type: "POST",
			url: "/Budget",
			data: "month=" + month + "&categoryId=" + catId + "&amount=" + amount,
			success: function (response) {
				$(td).find('span:not(.edit-mode)').text(response['total']);
				$(td).find('span:.edit-mode input').attr('value', response['extra']);
				if (response['hasExtra']) {
					$(td).addClass('extra');
				}
				else {
					$(td).removeClass('extra');
				}

			}
		});
		$(td).toggleClass('editing');
	});

	$('#budget-account').change(function () {
		var accountId = $(this).find("option:selected").val();
		var accountUrl = accountId == 0 ? "" : ("?accountId=" + accountId);
		window.location.href = '/Budget' + accountUrl;
	});

	$('#category-account').change(function () {
		var accountId = $(this).find("option:selected").val();
		var accountUrl = accountId == 0 ? "" : ("?accountId=" + accountId);
		window.location.href = '/Categories' + accountUrl;
	});

	$('#IncludeSecondaryDates').change(function () {
		$('.secondary-dates').toggle();
	});

	$('.bill-transactions .bill-amount input').blur(function () {
		var td = $(this).parents('td');
		var tr = $(this).parents('tr');
		var billTrxId = $(tr).data('bill-trx-id');
		var amount = $(this).val();

		$.ajax({
			type: "POST",
			url: "/BillTransaction",
			data: "billTransactionId=" + billTrxId + "&amount=" + amount
		});
		$(td).find('span:not(.edit-mode)').text(amount);
	});

	$('.bill-transactions .edit-mode .dp').datepicker({
		changeMonth: true,
		changeYear: true,
		dateFormat: 'yy-mm-dd',
		onClose: function (dateText, inst) {
			var td = $(this).parents('td');
			var tr = $(this).parents('tr');
			var billTrxId = $(tr).data('bill-trx-id');

			$.ajax({
				type: "POST",
				url: "/BillTransaction",
				data: "billTransactionId=" + billTrxId + "&date=" + dateText
			});
		}
	});

	$('.transactions .vendor select').change(function () {
		var sel = $(this);
		var vendorId = $(this).find("option:selected").val();
		var vendor = $(this).find("option:selected").html();
		var tr = $(this).parents('tr');
		var trxId = $(tr).data('trx-Id');

		$.ajax({
			type: "POST",
			url: "/Transaction/ChangeVendor",
			data: "transactionId=" + trxId + "&vendorId=" + vendorId,
			complete: function () {
				$(tr).data('vendor-Id', vendorId);
			}
		});
	});

	$('.transactions .category select').change(function () {
		var sel = $(this);
		var catId = $(this).find("option:selected").val();
		var cat = $(this).find("option:selected").html();
		var tr = $(this).parents('tr');
		var trxId = $(tr).data('trx-Id');

		$.ajax({
			type: "POST",
			url: "/Transaction/ChangeCategory",
			data: "transactionId=" + trxId + "&categoryId=" + catId,
			complete: function () {
				$(tr).data('category-Id', catId);
			}
		});
	});

	$('.transactions .edit-mode .dp').datepicker({
		changeMonth: true,
		changeYear: true,
		dateFormat: 'yy-mm-dd',
		onClose: function (dateText, inst) {
			$.ajax({
				type: "POST",
				url: "/Transaction/ChangeDate",
				data: "transactionId=" + $(this).parents('tr').data('trx-id') + "&date=" + dateText
			});
		}
	});

	$('.dp').datepicker({
		changeMonth: true,
		changeYear: true,
		dateFormat: 'yy-mm-dd'
	});

	var fromMonth;
	var toMonth;
	$('.month-nav:not(.just-links) td').not('.year').mousedown(function () {
		if (fromMonth === undefined) {
			fromMonth = $(this);
			$('.month-nav td').removeClass('selected');
			$(fromMonth).addClass('selected');

			$('.month-nav td').mouseenter(function () {
				var enterMonth = $(this);
				var fromIndex = $('.month-nav td').index(fromMonth);
				var enterIndex = $('.month-nav td').index(enterMonth);
				var min, max;
				if (fromIndex < enterIndex) {
					min = fromIndex;
					max = enterIndex + 1;
				}
				else {
					min = enterIndex;
					max = fromIndex + 1;
				}

				var sel = $('.month-nav td').slice(min, max);
				$('.month-nav td').not('.year').not(sel).removeClass('selected');
				$(sel).not('.year').addClass('selected');
			});
		}
		else {
			toMonth = $(this);
			$('.month-nav td').removeClass('selected');
			var fromIndex = $('.month-nav td').index(fromMonth);
			var toIndex = $('.month-nav td').index(toMonth);
			var min, max;
			if (fromIndex < toIndex) {
				min = fromIndex;
				max = toIndex + 1;
			}
			else {
				var temp = fromMonth;
				fromMonth = toMonth;
				toMonth = temp;
				min = toIndex;
				max = fromIndex + 1;
			}

			var sel = $('.month-nav td').slice(min, max);
			$('.month-nav td').not('.year').not(sel).removeClass('selected');
			$(sel).not('.year').addClass('selected');

			var fromDate = $(fromMonth).data('from');
			var toDate = $(toMonth).data('to');
			var navLinkUrl = (typeof (monthNavUrl) == 'undefined') ? "Transactions" : monthNavUrl;
			var accountParam = (typeof (urlAccountId) == 'undefined') ? "" : "&accountId=" + urlAccountId;
			var categoryParam = (typeof (urlCategoryId) == 'undefined') ? "" : "&categoryId=" + urlCategoryId;
			var vendorParam = (typeof (urlVendorId) == 'undefined') ? "" : "&vendorId=" + urlVendorId;
			var params = accountParam + categoryParam + vendorParam + '&from=' + fromDate + '&to=' + toDate;
			if (params[0] == '&') {
				params = params.substring(1);
			}
			$('#month-nav-link').attr('href', '/' + navLinkUrl + '?' + params);
			$('#month-nav-link').text('View ' + navLinkUrl + ' from ' + fromDate + ' to ' + toDate);

			fromMonth = undefined;
			toMonth = undefined;
			$('.month-nav td').unbind('mouseenter');
		}
	});

});

function reconcile(id) {
	var selector = "#trx-" + id;
	$(selector).toggleClass('rec');
	var checkbox = $(selector).find(':checkbox');
	$(checkbox).attr('checked', !$(checkbox).attr('checked'));
}

function billPaid(id) {
	var selector = "#bill-trx-" + id;
	$(selector).toggleClass('rec').show();
	var checkbox = $(selector).find(':checkbox');
	$(checkbox).attr('checked', !$(checkbox).attr('checked'));
	$(selector).next('tr.bill-prediction').remove();
}

function billDeleted() {
	window.location.href = '/Bills';
}

function vendorDeleted() {
	window.location.href = '/Vendors';
}

function categoryDeleted() {
	window.location.href = '/Categories';
}