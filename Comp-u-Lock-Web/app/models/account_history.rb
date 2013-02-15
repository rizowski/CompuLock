class AccountHistory < ActiveRecord::Base
  attr_accessible :account_id, :domain, :title, :url, :visit_count

  validates :account_id, presence: true
  validates :domain, presence: true, uniqueness: {scope: :account_id}
  
  belongs_to :account
end
